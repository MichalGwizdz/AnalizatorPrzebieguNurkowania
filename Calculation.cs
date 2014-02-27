/*  
    Analizator Nurkowania służy do analizy nasycenia tkanek gazami obojętnymi dla zadanego profilu nurkowania
    Copyright (C) 2014  Michal Gwizdz (MIG)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

/* File Version:
 *  v1. Michal Gwizdz (MIG) 11.02.2015 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizatorNurkowaniaWFA
{
   /// <summary>
   /// Klasa licząca nasycenia
   /// </summary>
   class Calculation
   {
      /// <summary>Indeks maksymany przy przeszukiwaniu rezultaów</summary>
      private int upperResultSerch;
      /// <summary>Indeks minimalny przy przeszukiwaniu rezultaów</summary>
      private int lowerResultSerch;
      /// <summary>Dla nurkowań z mieszankami helowymi</summary>
      private bool bTmx;
      /// <summary>Zestaw paramertrów nurkowania</summary>
      Data.Diving calcDiving;
      /// <summary>Zestaw tkanek</summary>
      public Compartments.ListOfCompartment compartments;
      /// <summary>Wyniki - ciśnienia parcjalne w tkanakach</summary>
      public List<Result.PPraeasureForCompartment> result;
      /// <summary>
      /// Tworzy nowe obliczenia
      /// </summary>
      /// <param name="diving">Zestaw paramertrów nurkowania</param>
      /// <param name="type">Zestaw tkanek teoretycznych które będą brane pod uwagę</param>
      public Calculation(ref Data.Diving diving, Compartments.CompartmentsType type)
      {
         // Naczytujemy zestaw tkanek na potzreby obliczeń
         compartments = Compartments.ListOfCompartment.createListOfCompartment(type);
         if (diving.IsTmxDiving())
         {
            if (compartments.IsHeliumData())
            {
               compartments = null;
               throw new Exception("Nurkowanie z użyciem mieszanek tlenowych dla zestawu tkanek teoretycznych nie zawierajacych danych dotyczących helu");
            }
            else
               bTmx =  true;
         }
         calcDiving = diving;
         upperResultSerch = 0;
         lowerResultSerch = 0;
      }
      /// <summary>
      /// Uruchomienie obliczeń
      /// </summary>
      public void Run()
      {
          Data.ProfileData profile = calcDiving.GetProfileData();
          int compNo = compartments.compartment.Count();
          Result.PPraeasureForCompartment ppc;
          result = new List<Result.PPraeasureForCompartment>();
          double minHT = compartments.compartment.Min(s => s.HalfTimeN2);
          for ( int i = 0; i < profile.Count(); i++)
          {
             //czas początku segmentu
             double iT = profile.getInitTime(i);
             //czas trwania segmentu
             double pT = profile.getTime(i);
             //przyrost ciśnienia pomiędzy początkiem i końcem segmentu
             double dP = profile.getPreasure(iT+pT,i) - profile.getPreasure(iT,i);
             //Punkt obliczeniowy minimum co 1/10 najkróteszego półokresu tkanek
             double dT = 0.1*minHT;
             //Nie mniej niż 1/4 czasu trwania segmentu
             dT = Math.Min(dT,0.25*pT);
             //Obliczamy ilość kroków zaookrąglając w górę
             int step = (int)Math.Floor(pT/dT);
             //Krok nie rzadziej niż zmiana ciśnienia o 1bar
             if (dP.CompareTo(0.0) != 0)
                step = (int)Math.Max(step, Math.Floor(1.0 / dP));
             //Finalny krok czasowy
             dT =  pT / step;
             // Dla zadanego segmentu liczymy cisnienia w kolejnych krokach
             for (int j = 0; j < step; j++)
             {
                // Rezulataty dla segmentu i czasu iT
                // Miejsce na nowy zestaw wyników
                ppc = new Result.PPraeasureForCompartment(compNo);
                // Bieżący czas
                ppc.Time = iT;
                // Bieżące ciśnienie zewnętrzne
                ppc.AmbientPreasure = profile.getPreasure(iT, i);
                // Bieżące ciśnienie parcjalne azotu
                ppc.PreasureN2 = profile.getPartialPreasureN2(iT, i);
                // Bieżące ciśnienie parcjalne helu
                if(bTmx)
                  ppc.PreasureHe = profile.getPartialPreasureHe(iT, i);
                // Bieżąca głębokość
                ppc.Depth = profile.getDepth(iT, i);
                // Obliczanie nasycenia tkanki gazami obojętnymi dla każdej z tkanek, oraz minimalnego ciśnienia akceptowalnego przez tkankę
                
                //Wersja wielowatkowa - uruchomić po testach jednowątkowej!
                //Parallel.For(0, compNo, k =>
                //{
                //   double ceiling = 0;
                //   if (result.Count() == 0)
                //   {
                //      ppc.setN2(k, PartialPreasure(ppc.PreasureN2, 0.79 * calcDiving.AtmosphericPressure, 0, compartments.compartment[k].HalfTimeN2));
                //      if (bTmx)
                //         ppc.setN2(k, PartialPreasure(ppc.PreasureHe, 0.0, 0, compartments.compartment[k].HalfTimeHe));
                //   }
                //   else
                //   {
                //      ppc.setN2(k, PartialPreasure(ppc.PreasureN2, result.Last().getN2(k), dT, compartments.compartment[k].HalfTimeN2));
                //      if (bTmx)
                //         ppc.setN2(k, PartialPreasure(ppc.PreasureHe, result.Last().getHe(k), dT, compartments.compartment[k].HalfTimeHe));
                //   }
                //   if (bTmx)
                //      ceiling = ceiling = Ceiling(ppc.getN2(k), compartments.compartment[k].M0N2, compartments.compartment[k].dMN2, ppc.getHe(k), compartments.compartment[k].M0He, compartments.compartment[k].dMHe);
                //   else
                //      ceiling = Ceiling(ppc.getN2(k), compartments.compartment[k].M0N2, compartments.compartment[k].dMN2);
                //   ppc.setCeiling(k,ceiling);
                //});
                
                // Obliczenia jednowątkowe
                for (int k = 0; k < compNo; k++)
                {
                   double ceiling = 0;
                   if (result.Count() == 0)
                   {
                      ppc.setN2(k, PartialPreasure(ppc.PreasureN2, 0.79 * calcDiving.AtmosphericPressure, 0, compartments.compartment[k].HalfTimeForN2(false)));
                      if (bTmx)
                         ppc.setN2(k, PartialPreasure(ppc.PreasureHe, 0.0, 0, compartments.compartment[k].HalfTimeForHe(false)));
                   }
                   else
                   {
                      ppc.setN2(k, PartialPreasure(ppc.PreasureN2, result.Last().getN2(k), dT, compartments.compartment[k].HalfTimeForN2(ppc.PreasureN2<result.Last().getN2(k))));
                      if (bTmx)
                         ppc.setN2(k, PartialPreasure(ppc.PreasureHe, result.Last().getHe(k), dT, compartments.compartment[k].HalfTimeForHe(ppc.PreasureHe < result.Last().getHe(k))));
                   }
                   if (bTmx)
                      ceiling = ceiling = Ceiling(ppc.getN2(k), compartments.compartment[k].M0N2, compartments.compartment[k].dMN2, ppc.getHe(k), compartments.compartment[k].M0He, compartments.compartment[k].dMHe);
                   else
                      ceiling = Ceiling(ppc.getN2(k), compartments.compartment[k].M0N2, compartments.compartment[k].dMN2);
                   ppc.setCeiling(k,ceiling);
                }
                // koniec petli jednowątkowej
                result.Add(ppc);
                iT += dT;
             }
          }
          lowerResultSerch = 0;
          upperResultSerch = (int)(0.5*result.Count());
      }
      public Result.PPraeasureForCompartment GetMaxValues(bool tmx)
      {
         int compNo = compartments.compartment.Count();
         Result.PPraeasureForCompartment res = new Result.PPraeasureForCompartment(compNo, tmx);
         foreach (Result.PPraeasureForCompartment ppfc in result)
         {
            res.AmbientPreasure = Math.Max(res.AmbientPreasure, ppfc.AmbientPreasure);
            res.Depth = Math.Max(res.Depth, ppfc.Depth);
            res.PreasureN2 = Math.Max(res.PreasureN2, ppfc.PreasureN2);
            if(tmx)
             res.PreasureHe = Math.Max(res.PreasureHe, ppfc.PreasureHe);
            res.Time = Math.Max(res.Time, ppfc.Time);
            int count = ppfc.Count();
            for (int i = 0; i < count; i++)
            {
               res.setN2(i, Math.Max(res.getN2(i), ppfc.getN2(i)));
               if(tmx)
                  res.setHe(i, Math.Max(res.getHe(i), ppfc.getHe(i)));
            }
         }
         return res;
      }
      public Result.PPraeasureForCompartment GetValusAtTime(double time, bool tmx)
      {
          int compNo = compartments.compartment.Count();
          Result.PPraeasureForCompartment res = new Result.PPraeasureForCompartment(compNo, tmx);
          if (result[upperResultSerch].Time < time)
          {
              lowerResultSerch = upperResultSerch;
              upperResultSerch = result.Count()-1;
              if(result[upperResultSerch].Time < time)
                  throw new Exception("GetValusAtTime. Invalid time, over maximum value");
          }
          if (result[lowerResultSerch].Time > time)
          {
              upperResultSerch = lowerResultSerch;
              lowerResultSerch = 0;
              if (result[lowerResultSerch].Time > time)
                  throw new Exception("GetValusAtTime. Invalid time, under minimum value");
          }
          if (lowerResultSerch > upperResultSerch)
              throw new Exception("GetValusAtTime. Invalid range");
          bool notFound = true;
          int curentIndex = 0;
          while (notFound)
          {
              if (Math.Abs(result[lowerResultSerch].Time - time) < Double.Epsilon)
              {
                  upperResultSerch = lowerResultSerch;
              }
              else if (Math.Abs(result[upperResultSerch].Time - time) < Double.Epsilon)
              {
                  lowerResultSerch = upperResultSerch;
              }
              else
              {
                  curentIndex = (int)(0.5 * (upperResultSerch + lowerResultSerch));
                  if (result[curentIndex].Time < time)
                      lowerResultSerch = curentIndex;
                  else
                      upperResultSerch = curentIndex;
              }
              notFound = (upperResultSerch - lowerResultSerch) > 1;
          }
          if (upperResultSerch == lowerResultSerch)
          {
              res = result[upperResultSerch];
          }
          else
          {
              double delta = (time - result[lowerResultSerch].Time)/ (result[upperResultSerch].Time - result[lowerResultSerch].Time);
              res.Time = time;
              res.AmbientPreasure = result[lowerResultSerch].AmbientPreasure + delta * (result[upperResultSerch].AmbientPreasure - result[lowerResultSerch].AmbientPreasure);
              res.Depth = result[lowerResultSerch].Depth + delta * (result[upperResultSerch].Depth - result[lowerResultSerch].Depth);
              for (int i = 0; i < compNo; i++)
              {
                  res.setN2(i,(result[lowerResultSerch].getN2(i) + delta * (result[upperResultSerch].getN2(i) - result[lowerResultSerch].getN2(i))));
                  if(tmx)
                      res.setHe(i, (result[lowerResultSerch].getHe(i) + delta * (result[upperResultSerch].getHe(i) - result[lowerResultSerch].getHe(i))));
              }
          }
          
          return res;
      }
      private double PartialPreasure(double Pa, double P0, double time, double halftime)
      {
          return P0 + (Pa - P0) * (1.0 - Math.Pow(2, -time / halftime));
      }
      private double Ceiling(double pCompartment, double M0, double dM)
      {
          double apmsw = 10.0*calcDiving.AtmosphericPressure;
          double a = M0 - dM * apmsw;
          double b = 1/dM;
          return (pCompartment - 0.1*a) * b;
      }
      private double Ceiling(double pCompartmentFirst, double M0First, double dMFirst, double pCompartmentSecond, double M0Second, double dMSecond)
      {
         double apmsw = 10.0 * calcDiving.AtmosphericPressure;
         double pCompartment = pCompartmentFirst + pCompartmentSecond;
         double a = ((M0First - dMFirst * apmsw)*pCompartmentFirst + (M0Second - dMSecond * apmsw)*pCompartmentSecond)/pCompartment;
         double b = (pCompartmentFirst / dMFirst) + (pCompartmentSecond/dMSecond) / pCompartment;
         return (pCompartment - 0.1 * a) * b;
      }
   }

}
