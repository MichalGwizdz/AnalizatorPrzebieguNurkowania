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
using System.Xml.Serialization;


namespace Compartments
{   
   ///<summary>Typ służący do identyfikacji zestawów parametrów tkanek</summary>
   public enum CompartmentsType
   {
      /// <summary>Zestaw tkanki według R. Workman rok 1965</summary>
      Workman,
      /// <summary>Zestaw ZHL-12 według A. Bühlmann compartments rok 1983</summary>
      ZHL12,
      /// <summary>"Zestaw  MF 11F6 Decompression Computation and Analysis Program (DCAP) Hamilton Research's rok 1998"</summary>
      DCAP_MF11F6,
      /// <summary>Zestaw "A" ZHL-16 według A. Bühlmann compartments rok 1990</summary>
      ZHL16A,
      /// <summary>Zestaw "B" ZHL-16 według A. Bühlmann compartments rok 1990</summary>
      ZHL16B,
      /// <summary>Zestaw "C" ZHL-16 według A. Bühlmann compartments rok 1990</summary>
      ZHL16C,
   }
   /// <summary>
   /// Klasa opisujaca tkankę teortyczną
   /// </summary>
   [XmlRoot("Compartment")]
   public class Compartment
   {
     /// <summary>Nazwa tkanki</summary>
     string name;
     /// <summary>Definuje czy półokresy dla nasycenia i odsycenia są symetryczne</summary>
     double halfTimeN2;
     /// <summary>Dla niesymetrycznych półokres odsycania dla azotu</summary>
     double halfTimeDesatN2;
     /// <summary>Wartość M0 dla azotu</summary>
     double cfM0N2;
     /// <summary>Wartość deltaM dla azotu</summary>
     double cfdMN2;
     /// <summary>Półokres dla helu, dla niesymetrycznych półokres nasycania</summary>
     double halfTimeHe;
     /// <summary>Dla niesymetrycznych półokres odsycania</summary>
     double halfTimeDesatHe;
     /// <summary>Wartość M0 dla azotu</summary>
     double cfM0He;
     /// <summary>Wartość deltaM dla helu</summary>
     double cfdMHe;
     
     /// <summary>
     /// Domyslny konstuktor dla tkanki teoretycznej, może się kiedyś przydać
     /// </summary>
     public Compartment()
     {
        name = "default";
        halfTimeN2 = 0;
        halfTimeDesatN2 = 0;
        cfM0N2 = 0;
        cfdMN2 = 0;
        halfTimeHe = 0;
        halfTimeDesatHe = 0;
        cfM0He = 0;
        cfdMHe = 0;
     }
     /// <summary>
     /// Konstuktor dla tkanki teoretycznej z tylko parametrami dla azotu z symetrycznym procesem nasycenie-odsycenie
     /// </summary>
     public Compartment( string compartmentName, double compartmentHalfTime, double compartmentM0 , double compartmentdM)
     {
         name = compartmentName;
         halfTimeN2 = compartmentHalfTime;
         halfTimeDesatN2 = -1;
         cfM0N2 = compartmentM0;
         cfdMN2 = compartmentdM;
         halfTimeHe = -1;
         halfTimeDesatHe = -1;
         cfM0He = -1;
         cfdMHe = -1;
     }
     /// <summary>
     /// Konstuktor dla tkanki teoretycznej z tylko parametrami dla azotu z nie z symetrycznym procesem nasycenie-odsycenie
     /// </summary>
     public Compartment(string compartmentName, double compartmentHalfTime, double compartmentHalfDesatTime, double compartmentM0, double compartmentdM)
     {
        name = compartmentName;
        halfTimeN2 = compartmentHalfTime;
        halfTimeDesatN2 = compartmentHalfDesatTime;
        cfM0N2 = compartmentM0;
        cfdMN2 = compartmentdM;
        halfTimeHe = -1;
        halfTimeDesatHe = -1;
        cfM0He = -1;
        cfdMHe = -1;
     }
     /// <summary>
     /// Konstuktor dla tkanki teoretycznej z parametrami dla azotu i helu z symetrycznym procesem nasycenie-odsycenie
     /// </summary>
     public Compartment(string compartmentName, double compartmentHalfTimeN2, double compartmentM0N2, double compartmentdMN2, double compartmentHalfTimeHe, double compartmentM0He, double compartmentdMHe)
     {
        name = compartmentName;
        halfTimeN2 = compartmentHalfTimeN2;
        halfTimeDesatN2 = -1;
        cfM0N2 = compartmentM0N2;
        cfdMN2 = compartmentdMN2;
        halfTimeHe = compartmentHalfTimeHe;
        halfTimeDesatHe = -1;
        cfM0He = compartmentM0He;
        cfdMHe = compartmentdMHe;
     }
     /// <summary>
     /// Konstuktor dla tkanki teoretycznej z parametrami dla azotu i helu z niesymetrycznym procesem nasycenie-odsycenie
     /// </summary>
     public Compartment(string compartmentName, double compartmentHalfTimeN2, double compartmentHalfTimeDesatN2, double compartmentM0N2, double compartmentdMN2, double compartmentHalfTimeHe, double compartmentHalfTimeDesatHe, double compartmentM0He, double compartmentdMHe)
     {
        name = compartmentName;
        halfTimeN2 = compartmentHalfTimeN2;
        halfTimeDesatN2 = compartmentHalfTimeDesatN2;
        cfM0N2 = compartmentM0N2;
        cfdMN2 = compartmentdMN2;
        halfTimeHe = compartmentHalfTimeHe;
        halfTimeDesatHe = compartmentHalfTimeDesatHe;
        cfM0He = compartmentM0He;
        cfdMHe = compartmentdMHe;
     }
     /// <summary>
     /// Nazwa tkanki - ułatwia identyfikacje
     /// </summary>
     public string Name
     {
          get { return name;}
     }
     /// <summary>
     /// Półokres dla nasycenia i odsycenia azotem, również dla procesu niesymetrycznego gdy jest ustawionyHalfTimeDesaturationN2
     /// </summary>
     public double HalfTimeForN2(bool destauration)
     {
        if(halfTimeDesatN2>0)
        {
           return destauration ? halfTimeDesatN2 : halfTimeN2;
        }
        else
           return halfTimeN2;
     }
     /// <summary>
     /// Półokres dla nasycenia azotem (i odsycenia o ile proces jest symetryczny czyli nie został ustaiwiony HalfTimeDesaturationN2
     /// </summary>
     public double HalfTimeN2
     {
          get { return halfTimeN2;}
     }
     /// <summary>
     /// Półokres dla odsycania azotu - opcjonalnie dla niesymetrycznego procesu
     /// </summary>
     public double HalfTimeDesaturationN2
     {
        get { return halfTimeDesatN2 > 0.0 ? halfTimeDesatN2 : halfTimeN2; }
     }
     /// <summary>
     /// M0 dla równaia M = M0 + △M * h  dla azotu
     /// </summary>
     public double M0N2
     {
      get { return cfM0N2;}
     }
     /// <summary>
     /// △M dla równaia M = Mo + △M * h dla azotu
     /// </summary>
     public double dMN2
     {
         get { return cfdMN2;}
     }
     /// <summary>
     /// Półokres dla nasycenia i odsycenia helem, również dla procesu niesymetrycznego gdy jest ustawiony HalfTimeDesaturationHe
     /// </summary>
     public double HalfTimeForHe(bool destauration)
     {
        if (halfTimeDesatHe > 0)
        {
           return destauration ? halfTimeDesatHe : halfTimeHe;
        }
        else
           return halfTimeHe;
     }
     /// <summary>
     /// Półokres dla nasycenia helem (i odsycenia o ile proces jest symetryczny czyli nie został ustaiwiony HalfTimeDesaturationHe
     /// </summary>
     public double HalfTimeHe
     {
         get { return halfTimeHe; }
     }
     /// <summary>
     /// Półokres dla odsycania helu - opcjonalnie dla niesymetrycznego procesu
     /// </summary>
     public double HalfTimeDesaturationHe
     {
        get { return halfTimeDesatHe > 0.0 ? halfTimeDesatHe : halfTimeHe; }
     }
     /// <summary>
     /// M0 dla równaia M = M0 + △M * h dla helu
     /// </summary>
     public double M0He
     {
         get { return cfM0He; }
     }
     /// <summary>
     /// △M dla równaia M = Mo + △M * h dla helu
     /// </summary>
     public double dMHe
     {
         get { return cfdMHe; }
     }
   }
  /// <summary>
  /// Klasa opisujaca zestwa tkankek teortycznych do obliczeń
  /// </summary>
   public class ListOfCompartment
   {
      /// <summary>
      /// Zestaw tkanek teortycznych
      /// </summary>
      public List<Compartment> compartment = new List<Compartment>();
      /// <summary>
      /// Opis/nazwa zestawu tkanek
      /// </summary>
      protected string longDescription;
      /// <summary>
      /// Identyfikator - krótka nazwa zestawu tkanek teortycznych
      /// </summary>
      protected string shortDescription;
      /// <summary>
      /// Zwraca skrócony opis/nazwa zestawu tkanek teortycznych
      /// </summary>
      public string Description
      { 
          get { return longDescription;}
      }
      /// <summary>
      /// Zwraca identyfikator - krótką nazwę zestawu tkanek teortycznych
      /// </summary>
      public string ID
      { 
          get { return shortDescription;}
      }
      /// <summary>
      /// Informacja o danych dla helu
      /// </summary>
      /// <returns>true - gdy są dane umożliwiające obliczenia dla mieszanek helowych, false w przeciwnym przypadku</returns>
      public bool IsHeliumData()
      {
          bool heliumData = false;
          double val = compartment.Min(s => s.HalfTimeHe);
          if (val > Double.Epsilon)
          {
              val = compartment.Min(s => s.M0He);
              if (val > Double.Epsilon)
              {
                  val = compartment.Min(s => s.dMHe);
                  heliumData = true;
              }
          }
          return heliumData;
      }
      /// <summary>
      /// Informacja o symetrycznosci procesu nasycenie - odsycenie
      /// </summary>
      /// <returns>true - gdy tkanki teortyczne są symetryczne, false w przeciwnym przypadku</returns>
      public bool IsSymetric()
      {
         bool IsSymetric = true;
         if (IsHeliumData())
         {
            double val = compartment.Min(s => s.HalfTimeDesaturationHe);
            if (val > Double.Epsilon)
               IsSymetric = false;
         }
         if (IsSymetric)
         {
            double val = compartment.Min(s => s.HalfTimeDesaturationN2);
            if (val > Double.Epsilon)
                  IsSymetric = false;
         }
         return IsSymetric;
      }
      /// <summary>
      /// Domyslny konstruktor - może być przydany przy serializacji
      /// </summary>
      public ListOfCompartment()
      {
      }
      /// <summary>
      /// Konmstruktor tworzacy typowe zestawy tkanek teortycznych
      /// </summary>
      /// <param name="type">Typ zestawu według listy <see cref="CompartmentsType"/>CompartmentsType</see></param>
      /// <returns>Zwraca jeden z typowych zestawów tkanek</returns>
      public static ListOfCompartment createListOfCompartment(CompartmentsType type)
      {
          ListOfCompartment newListOfCompartment = new ListOfCompartment();
          switch (type)
          {
              case CompartmentsType.Workman:
                  newListOfCompartment = new Workman1965();
                  break;
              case CompartmentsType.ZHL12:
                  newListOfCompartment = new ZHL12();
                  break;
              case CompartmentsType.DCAP_MF11F6:
                  newListOfCompartment = new DCAP_MF11F6();
                  break;
              case CompartmentsType.ZHL16A:
                  newListOfCompartment = new ZHL16(ZHL16.M0Type.A);
                  break;
              case CompartmentsType.ZHL16B:
                  newListOfCompartment = new ZHL16(ZHL16.M0Type.B);
                  break;
              case CompartmentsType.ZHL16C:
                  newListOfCompartment = new ZHL16(ZHL16.M0Type.C);
                  break;
          };
          return newListOfCompartment;
      }
   }
   /// <summary>
   /// Klasa zawierajaca zestaw tkanek teoretycznych według R. Workman rok 1965
   /// </summary>
   public class Workman1965 : ListOfCompartment
   {
      /// <summary>
      /// Tworzy zestaw tkanek teoretycznych według R. Workman rok 1965
      /// </summary>
      public Workman1965()
      {
          longDescription = "Workman compartment 1965";
          shortDescription = CompartmentsType.Workman.ToString();
          compartment.Add(new Compartment("1", 5, 31.7, 1.8));
          compartment.Add(new Compartment("2", 10, 26.8, 1.6));
          compartment.Add(new Compartment("3", 20, 21.9, 1.5));
          compartment.Add(new Compartment("4", 40, 17.0, 1.4));
          compartment.Add(new Compartment("5", 80, 16.4, 1.3));
          compartment.Add(new Compartment("6", 120, 15.8, 1.2));
          compartment.Add(new Compartment("7", 160, 15.5, 1.15));
          compartment.Add(new Compartment("8", 200, 15.5, 1.1));
          compartment.Add(new Compartment("9", 240, 15.2, 1.1));
      }
   }
   /// <summary>
   /// Klasa zawierajaca zestaw tkanek teoretycznych ZHL-12 według A. Bühlmann compartments rok 1983
   /// </summary>
   public class ZHL12 : ListOfCompartment
   {
      /// <summary>
      /// Tworzy zestaw tkanek teoretycznych ZHL-12 według A. Bühlmann compartments rok 1983
      /// </summary>
      public ZHL12()
      {
          longDescription = "Bühlmann compartments ZHL-12 1983";
          shortDescription = CompartmentsType.ZHL12.ToString();
          compartment.Add(new Compartment("1", 2.65, 34.2, 1.2195));
          compartment.Add(new Compartment("2", 7.94, 27.2, 1.2195));
          compartment.Add(new Compartment("3", 12.2, 22.9, 1.2121));
          compartment.Add(new Compartment("4", 18.5, 21.0, 1.1976));
          compartment.Add(new Compartment("5", 26.5, 19.3, 1.1834));
          compartment.Add(new Compartment("6", 37, 17.4, 1.1628));
          compartment.Add(new Compartment("7", 53, 16.2, 1.1494));
          compartment.Add(new Compartment("8", 79, 15.8, 1.1236));
          compartment.Add(new Compartment("9", 114, 15.8, 1.1236));
          compartment.Add(new Compartment("10", 146, 15.3, 1.0707));
          compartment.Add(new Compartment("11", 185, 15.3, 1.0707));
          compartment.Add(new Compartment("12", 238, 14.4, 1.0593));
          compartment.Add(new Compartment("13", 304, 12.9, 1.0395));
          compartment.Add(new Compartment("14", 397, 12.9, 1.0395));
          compartment.Add(new Compartment("15", 503, 12.9, 1.0395));
          compartment.Add(new Compartment("16", 635, 12.9, 1.0395));
      }
   }
   /// <summary>
   /// Klasa zawierajaca zestaw tkanek teoretycznych MF 11F6 Decompression Computation and Analysis Program (DCAP) Hamilton Research's rok 1998
   /// </summary>
   public class DCAP_MF11F6  : ListOfCompartment
   {
      /// <summary>
      /// Tworzy zestaw tkanek teoretycznych MF 11F6 Decompression Computation and Analysis Program (DCAP) Hamilton Research's rok 1998
      /// </summary>
      public DCAP_MF11F6()
      {
          longDescription = "Hamilton Research's Decompression Computation and Analysis Program (DCAP) MF 11F6";
          shortDescription = CompartmentsType.DCAP_MF11F6.ToString();
          compartment.Add(new Compartment("1", 5, 31.9, 1.3));
          compartment.Add(new Compartment("2", 10, 24.65, 1.05));
          compartment.Add(new Compartment("3", 25, 19.04, 1.08));
          compartment.Add(new Compartment("4", 55, 14.78, 1.06));
          compartment.Add(new Compartment("5", 95, 13.92, 1.04));
          compartment.Add(new Compartment("6", 145, 13.66, 1.02));
          compartment.Add(new Compartment("7", 200, 13.53, 1.01));
          compartment.Add(new Compartment("8", 285, 13.5, 1.0));
          compartment.Add(new Compartment("9", 385, 13.5, 1.0));
          compartment.Add(new Compartment("10", 520, 13.4, 1.0));
          compartment.Add(new Compartment("11", 670, 13.3, 1.0));
      }
   }
   /// <summary>
   /// Klasa zawierajaca zestaw tkanek teoretycznych ZHL-16 według A. Bühlmann compartments rok 1990
   /// </summary>
   public class ZHL16  : ListOfCompartment
   {
      ///<summary>Typ służący do identyfikacji zestawów parametrów tkanek w obrębie ZHL-16</summary>
      public enum M0Type
      {
         /// <summary>Zestaw A</summary>
         A,
         /// <summary>Zestaw B -  typowy</summary>
         B,
         /// <summary>Zestaw C</summary>
         C,
      }
      /// <summary>
      /// Tworzy zestaw tkanek teoretycznych ZHL-16 według A. Bühlmann compartments rok 1990
      /// </summary>
      public ZHL16(M0Type Mo)
      {
         // Najpierw tworzymy zestaw "B"
         ZHL16B();
         //Jeśli zestaw jest inny niż "B" nadpisujemy część danych
         if (Mo  == M0Type.A)
         {
             longDescription = "Bühlmann compartments ZHL-16 A 1990";
             shortDescription = CompartmentsType.ZHL16A.ToString();
             compartment[7] = new Compartment("6", 38.3, 17.8, 1.1857);
             compartment[8] = new Compartment("7", 54.3, 16.8, 1.1504);
             compartment[9] = new Compartment("8", 77, 15.9, 1.1223);
             compartment[14] = new Compartment("13", 305, 13.5, 1.0552);
         }
         else if (Mo  == M0Type.C)
         {
             longDescription = "Bühlmann compartments ZHL-16 C 1990";
             shortDescription = CompartmentsType.ZHL16C.ToString();
             compartment[6] = new Compartment("5", 27, 18.5, 1.2306);   
             compartment[7] = new Compartment("6", 38.3, 16.9, 1.1857);
             compartment[8] = new Compartment("7", 54.3, 15.9, 1.1504);
             compartment[9] = new Compartment("8", 77, 15.2, 1.1223);
             compartment[10] = new Compartment("9", 109, 14.7, 1.0999);
             compartment[11] = new Compartment("10", 146, 14.3, 1.0844);
             compartment[12] = new Compartment("11", 187, 14.0, 1.0731);
             compartment[13] = new Compartment("12", 239, 13.7, 1.0635);
             compartment[15]=  new Compartment("14", 390, 13.1, 1.0478);
             }
      }
      /// <summary>
      /// Tworzy zestaw tkanek teoretycznych "B" ZHL-16 według A. Bühlmann compartments rok 1990
      /// </summary>
      public void ZHL16B()
      {
          longDescription = "Bühlmann compartments ZHL-16 B 1990";
          shortDescription = CompartmentsType.ZHL16B.ToString();
          compartment.Add(new Compartment("1", 4, 32.4, 1.9082));
          compartment.Add(new Compartment("1a", 5, 29.6, 1.7928));
          compartment.Add(new Compartment("2", 8, 25.4, 1.5352));
          compartment.Add(new Compartment("3", 12.5, 22.5, 1.3847));
          compartment.Add(new Compartment("4", 18.5, 20.3, 1.278));
          compartment.Add(new Compartment("5", 27, 19.0, 1.2306));   
          compartment.Add(new Compartment("6", 38.3, 17.5, 1.1857));
          compartment.Add(new Compartment("7", 54.3, 16.5, 1.1504));
          compartment.Add(new Compartment("8", 77, 15.7, 1.1223));
          compartment.Add(new Compartment("9", 109, 15.2, 1.0999));
          compartment.Add(new Compartment("10", 146, 14.6, 1.0844));
          compartment.Add(new Compartment("11", 187, 14.2, 1.0731));
          compartment.Add(new Compartment("12", 239, 13.9, 1.0635));
          compartment.Add(new Compartment("13", 305, 13.4, 1.0552));
          compartment.Add(new Compartment("14", 390, 13.2, 1.0478));
          compartment.Add(new Compartment("15", 498, 12.9, 1.0414));
          compartment.Add(new Compartment("16", 635, 12.7, 1.0359));
      }
   }
}