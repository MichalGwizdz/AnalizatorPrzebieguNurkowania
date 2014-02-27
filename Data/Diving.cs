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
using Tools;

namespace Data
{
   /// <summary>
   /// Klasa opisuje przebieg całego nurkowania
   /// </summary>
   [XmlRoot("Diving")]
   public class Diving
   {
      /// <summary>
      /// Lista dostepnych gazów 
      /// </summary>
      [XmlIgnore]
      private List<GasMix> gasesList { get; set; }
      /// <summary>
      /// Lista segmentów
      /// </summary>
      [XmlIgnore]
      private List<DiveSegment> segmentsList { get; set; }
      /// <summary>
      /// Cięzar właściwy wody
      /// </summary>
      [XmlAttribute("WaterSpecificWeight")]
      public double WaterSpecificWeight { get; set; }
      /// <summary>
      /// Ciśnienie atmosferyczne
      /// </summary>
      [XmlAttribute("AtmosphericPressure")]
      public double AtmosphericPressure { get; set; }
      /// <summary>
      /// Prędkość zanurzania
      /// </summary>
      [XmlAttribute("DescentSpeed")]
      public double DescentSpeed { get; set; }
      /// <summary>
      /// Prędkość wynurzania
      /// </summary>
      [XmlAttribute("AscentSpeed")]
      public double AscentSpeed { get; set; }
      /// <summary>
      /// Domyslny konstruktor, tworzy typowe dane dla nurkowania
      /// </summary>
      public Diving()
      {
         gasesList = new List<GasMix>();
         segmentsList = new List<DiveSegment>();
         WaterSpecificWeight = 1000; // N/m3
         AtmosphericPressure = 1; // bar
         DescentSpeed = 20; // m/min
         AscentSpeed = 10; // m/min

      }
      /// <summary>
      /// Zwraca gęstość wody [kg/m3] lub ustawia ciężar właściwy [n/m3] na podstawie gęstości[kg/m3]
      /// </summary>
      [XmlIgnore]
      public double WaterDensity
      {
         get { return WaterSpecificWeight / Utility.gravity; }
         set { WaterSpecificWeight = value * Utility.gravity; }
      }
      /// <summary>
      /// Zwraca/ustawia listę segmentów
      /// </summary>
      public List<DiveSegment> SegmentsList
      {
         get { return segmentsList; }
         set { segmentsList = value; }
      }
      /// <summary>
      /// Zwraca/ustawia listę mieszanek gazów
      /// </summary>
      public List<GasMix> GasesList
      {
         get { return gasesList; }
         set { gasesList = value; }
      }
      /// <summary>
      /// Dodaje mieszankę do listy mieszanek gazów
      /// </summary>
      /// <param name="gas">Mieszanka gazów</param>
      /// <returns>true - gaz został dodany, false - gaz o tej nazwie juz istnieje dla tego nurkowania</returns>
      public bool AddGas(GasMix gas)
      {
         GasMix g = GetGas(gas.Name);
         if (g == null)
         {
            gasesList.Add(gas);
            return true;
         }
         else
            return false;
      }
      /// <summary>
      /// Usuwa gaz z listy mieszanek gazów
      /// </summary>
      /// <param name="gas">Mieszanka gazów</param>
      /// <returns>true - gaz został usunięty, false - gaz o tej nazwie nie istnieje dla tego nurkowania</returns>
      public bool RemoveGas(GasMix gas)
      {
         return gasesList.Remove(gas);
      }
      /// <summary>
      /// Zamienia gaz na liśie gazów
      /// </summary>
      /// <param name="newGas">Nowa mieszanka gazów</param>
      /// <param name="oldGas">Mieszanka gazów do usunięcia</param>
      /// <returns>true - gazy zostały zamienione, false - gaz do usunięcia nie istniał dla tego nurkowania</returns>
      public bool ReplaceGas(GasMix newGas, GasMix oldGas)
      {
         int i = gasesList.IndexOf(oldGas);
         if (i < 0)
            return false;
         else
         {
            gasesList[i] = newGas;
            return true;
         }
      }
      /// <summary>
      /// Usówa gaz z listy gazów
      /// </summary>
      /// <param name="gasName">Mieszanka gazów do usunięcia</param>
      /// <returns>true - gazy został usunięty, false - gaz do usunięcia nie istniał dla tego nurkowania</returns>
      public bool RemoveGas(string gasName)
      {
         GasMix gas = GetGas(gasName);
         if (gas != null)
            return RemoveGas(gas);
         else
            return false;
      }
      /// <summary>
      /// Zwraca mieszankę gazów na podstwie nazwy
      /// </summary>
      /// <param name="gasName">Nazwa mieszanki</param>
      /// <returns>Zwraca mieszankę gazów, lub null gdy nie istnieje taka mieszanka</returns>
      public GasMix GetGas(string gasName)
      {
         GasMix gas = null;
         try
         {
            gas = gasesList.Find(
               delegate(GasMix g)
               {
                  return g.Name == gasName;
               }
            );
         }
         catch (Exception e) { };

         return gas;
      }
      /// <summary>
      /// Głębokość maksymalna nurkowania
      /// </summary>
      /// <returns>Maksymalną głebokość nurkowania</returns>
      public double DepthMax()
      {
         return segmentsList.Max(s => Math.Max(s.InitialDepth, s.FinialDepth));
      }
      /// <summary>
      /// Średnia głebokość nurkowania
      /// </summary>
      /// <returns>Średnią głębokość nurkowania</returns>
      public double DepthAverage()
      {
         double time = 0;
         double timeDepth = 0;
         foreach (DiveSegment gm in segmentsList)
         {
            time += gm.Time;
            timeDepth += gm.Time * 0.5 * (gm.InitialDepth + gm.FinialDepth);
         }
         return timeDepth / time;
      }
      /// <summary>
      /// Dodaje segment nurkowania do listy segmentów
      /// </summary>
      /// <param name="segment">Segment który ma być dodany</param>
      /// <returns>true - segment został dodany, false - błąd, juz istnieje segment o tej nazwie</returns>
      public bool AddSegment(DiveSegment segment)
      {
         DiveSegment ds = segmentsList.Find(s => s.Name == segment.Name);
         if (ds == null)
         {
            segmentsList.Add(segment);
            return true;
         }
         else
            return false;
      }
      /// <summary>
      /// Usuwa segment nurkowania z listy segmentów
      /// </summary>
      /// <param name="segment">Segment który ma być usuniety</param>
      /// <returns>true - segment został usunięty, false - błąd, nie istnieje segment o tej nazwie</returns>
      public bool RemoveSegment(DiveSegment segment)
      {
         return segmentsList.Remove(segment);
      }
      /// <summary>
      /// Usuwa segment nurkowania z listy segmentów na podstawie indeksu
      /// </summary>
      /// <param name="index">Index na liście segmentów</param>
      /// <returns>true - segment został usunięty, false - błąd, indeks poza zakresem</returns>
      public bool RemoveSegment(int index)
      {
         if (index >= 0 && index < segmentsList.Count)
         {
            segmentsList.RemoveAt(index);
            return true;
         }
         else
            return false;
      }
      /// <summary>
      /// Podmienia segment nurkowania na liście segmentów
      /// </summary>
      /// <param name="newSegment">Segnemt który zostanie dodany w miejscu oldSegment</param>
      /// <param name="oldSegment">Segnemt który zostanie usunięty</param>
      /// <returns>true - segment został podmieniony, false - błąd, brak segmentu oldSegment na liśie segmentów</returns>
      public bool ReplaceSegment(DiveSegment newSegment, DiveSegment oldSegment)
      {
         int i = segmentsList.IndexOf(oldSegment);
         if (i < 0)
            return false;
         else
         {
            segmentsList[i] = newSegment;
            return true;
         }
      }
      /// <summary>
      /// Zwraca indeks segmentu na podstawie nazwy
      /// </summary>
      /// <param name="segmentName">Nazwa(identyfikator) segmentu</param>
      /// <returns>Indeks segmentu</returns>
      public int GetSegmentIndex(string segmentName)
      {
         return segmentsList.FindIndex(s => s.Name == segmentName);
      }
      /// <summary>
      /// Modyfikuje dane segmentu
      /// </summary>
      /// <param name="segmentName">Nazwa(identyfikator) segmentu</param>
      /// <param name="initialDepth">Głębokość poczatkowa [m]</param>
      /// <param name="finialDepth">Głębokość końcowa [m]</param>
      /// <param name="segmentTime">Czas trwania segmentu [min]</param>
      /// <param name="gas">Gaz w obrębie segmentu</param>
      /// <returns>Zwraca nazwę(identyfikator) segmentu po zmianie parametrów</returns>
      /// <exception cref="System.Exception">W razie problemów jest wyrzuacany wyjątek</exception>
      public string ChangeSegment(string segmentName, double initialDepth, double finialDepth, double segmentTime, GasMix gas = null)
      {
         string newName = segmentName;
         bool intDepthChage = false;
         bool finDepthChage = false;
         try
         {

            int index = segmentsList.FindIndex(s => s.Name == segmentName);
            segmentsList[index].Time = segmentTime;
            segmentsList[index].setDefaultName();
            newName = segmentsList[index].Name;
            intDepthChage = (Math.Abs(segmentsList[index].InitialDepth - initialDepth) > Double.Epsilon);
            finDepthChage = (Math.Abs(segmentsList[index].FinialDepth - finialDepth) > Double.Epsilon);
            if (gas != null)
            {
               AddGas(gas);
               segmentsList[index].GasName = gas.Name;
               segmentsList[index].setDefaultName();
               newName = segmentsList[index].Name;
            }
            if (intDepthChage || finDepthChage)
            {
               double deltaDepth = finialDepth - initialDepth;
               if (deltaDepth > 1e-3)
                  segmentsList[index].Time = deltaDepth / DescentSpeed;
               else if (deltaDepth < -1e-3)
                  segmentsList[index].Time = -deltaDepth / AscentSpeed;
               segmentsList[index].InitialDepth = initialDepth;
               segmentsList[index].FinialDepth = finialDepth;
               segmentsList[index].setDefaultName();
               newName = segmentsList[index].Name;
               RefactorSegmentsList(index, intDepthChage);
            }

         }
         catch (Exception)
         {
            throw new Exception("ChangeSegment(string segmentName, double initialDepth, double finialDepth, double segmentTime, GasMix gas = null): Invalid segment");
         }
         return newName;
      }
      /// <summary>
      /// Porządkuje liste segmentów po wartości głębokości poczatkowej
      /// </summary>
      /// <param name="startIndex">Indeks startowy porządkowania, dla watości domyślnej zostanie przyjety indeks poczatkowy listy</param>
      /// <param name="toBeginig">Flaga umożliwiajaca sortowanie od końca do poczatku</param>
      private void RefactorSegmentsList(int startIndex = -1, bool toBeginig = false)
      {
         startIndex = Math.Max(0, startIndex);
         List<int> segmentToRemove = new List<int>();
         if (toBeginig)
         {
            for (int i = startIndex; i > 0; i--)
            {
               if (segmentsList[i].InitialDepth < segmentsList[i - 1].InitialDepth)
               {
                  segmentToRemove.Add(i);
                  segmentsList[i - 1].InitialDepth = segmentsList[i].InitialDepth;
               }
               segmentsList[i - 1].FinialDepth = segmentsList[i].InitialDepth;
            }
         }
         else
         {
            int last_change = segmentsList.Count - 1;
            for (int i = startIndex; i < last_change; i++)
            {
               if (segmentsList[i].FinialDepth > segmentsList[i + 1].FinialDepth)
               {
                  segmentToRemove.Add(i + 1);
                  segmentsList[i + 1].FinialDepth = segmentsList[i].FinialDepth;
               }
               segmentsList[i + 1].InitialDepth = segmentsList[i].FinialDepth;

            }
         }
         if (segmentToRemove.Count > 0)
         {
            segmentToRemove.Reverse();
            foreach (int i in segmentToRemove)
            {
               segmentsList.RemoveAt(i);
            }
         };
      }
      /// <summary>
      /// Tworzy domyslny segment nurkowania i dodaje go do listy segmentów
      /// </summary>
      public void AddDefaultSegment()
      {
         DiveSegment segment = null;
         if (segmentsList.Count != 0)
         {
            double id = segmentsList[segmentsList.Count - 1].InitialDepth;
            double fd = segmentsList[segmentsList.Count - 1].FinialDepth;
            double t = 10;
            if (Math.Abs(fd - id) < 0.01)
               t += segmentsList[segmentsList.Count - 1].Time;
            segment = new DiveSegment(fd, fd, t, segmentsList[segmentsList.Count - 1].GasName);
         }
         else
         {
            if (gasesList.Count == 0)
               gasesList.Add(new GasMix());
            segment = new DiveSegment(0, 10, 10 / DescentSpeed, gasesList[0].Name);
         }
         segmentsList.Add(segment);
      }
      /// <summary>
      /// Zwraca runtime dla zadanego zegmentu czyli czas liczony od poczatku nurkowania do opuszczenia segmentu 
      /// </summary>
      /// <param name="segmentNo">Indeks segmentu, dla wartości domyślnej jest zwracany runtime dla ostatniego segmentu</param>
      /// <returns></returns>
      public double getRunTime(int segmentNo = -1)
      {
         double rt = 0;
         if (segmentNo < 0 || segmentNo >= segmentsList.Count())
            segmentNo = segmentsList.Count() - 1;
         for (int i = 0; i <= segmentNo; i++)
         {
            rt += segmentsList[i].Time;
         }
         return rt;
      }
      /// <summary>
      /// Tworzy profil nurkowania na podstawie listy segmentów
      /// </summary>
      /// <returns>Profil nurkowania czyli listę punktów na których beą przeprowadzane obliczenia</returns>
      public ProfileData GetProfileData()
      {
         ProfileData profileData = new ProfileData();
         GasMix currentGasMix = gasesList.First();
         double time = 0;
         double finialPreasure = 0;
         foreach (DiveSegment diveSegment in segmentsList)
         {
            if (diveSegment.GasName != currentGasMix.Name)
               currentGasMix = GetGas(diveSegment.GasName);
            ProfileData.profileData newData = new ProfileData.profileData();
            newData.initialT = time;
            newData.deltaT = diveSegment.Time;
            newData.initialD = diveSegment.InitialDepth;
            newData.deltaD = diveSegment.FinialDepth - diveSegment.InitialDepth;
            newData.initialP = PresureAtDepth(diveSegment.InitialDepth);
            finialPreasure = PresureAtDepth(diveSegment.FinialDepth);
            newData.deltaP = finialPreasure - newData.initialP;
            newData.initialPPN2 = newData.initialP * currentGasMix.Nitrogen;
            newData.deltaPPN2 = finialPreasure * currentGasMix.Nitrogen - newData.initialPPN2;
            newData.initialPPHe = newData.initialP * currentGasMix.Helium;
            newData.deltaPPHe = finialPreasure * currentGasMix.Helium - newData.initialPPHe;
            newData.initialPPO2 = newData.initialP * currentGasMix.Oxygen;
            newData.deltaPPO2 = finialPreasure * currentGasMix.Oxygen - newData.initialPPO2;
            profileData.Add(newData);
            time += diveSegment.Time;
         }
         return profileData;
      }
      /// <summary>
      /// Ciśnienia na zadanej głębokości
      /// </summary>
      /// <param name="depth">Głębokość [m]</param>
      /// <returns>Cisnienie z uwzględneienim gęstości wody i ćiśnienienia atmosferycznego [bar]</returns>
      public double PresureAtDepth(double depth)
      {
         return AtmosphericPressure + (depth * WaterSpecificWeight * 1e-4);
      }
      /// <summary>
      /// Głębokość odpowiadającą zadanemu ciśnieniu
      /// </summary>
      /// <param name="preasure">Ciśnienie [bar]</param>
      /// <returns>Głębokość na której panuje zadane ciśnienie  uwzględneienim gęstości wody i ćiśnienienia atmosferycznego [m]</returns>
      public double DepthAtPresure(double preasure)
      {
         return (preasure - AtmosphericPressure) / WaterSpecificWeight * 1e-4;
      }
      /// <summary>
      /// EAD - Eqivalent Air Depth - równoważna głębokość powietrzna
      /// </summary>
      /// <param name="gas">Mieszanka oddechowa</param>
      /// <param name="depth">Głębokość</param>
      /// <returns>Zwraca głebokość przy której cisnienie parcjalne azotu dla powietrza byłoby identyczne z ciśnieniem parcjalne azotu w mieszance dla zadanej głębokości</returns>
      /// <exception cref="System.Exception">Dla TMX jest wyrzuacany wyjątek</exception>
      public double EAD(GasMix gas, double depth)
      {
         if (gas.Helium > Double.Epsilon)
            throw new Exception("EAD is not available for TMX");
         return END(gas, 0.0);
      }
      /// <summary>
      /// END - Eqivalent Narcotic Depth - równoważna głębokość narkotyczna
      /// </summary>
      /// <param name="gas">Mieszanka oddechowa</param>
      /// <param name="depth">Głębokość</param>
      /// <param name="narcoticOxigenToNitrogen">Narkotycznośc tlenu w odniesieniu do azotu 0.0 - brak, 1.0 (wartość domyślna) tlen tak samo narkotyczny jak azot</param>
      /// <returns></returns>
      public double END(GasMix gas, double depth, double narcoticOxigenToNitrogen = 1.0)
      {
         double end = depth;
         if (gas.Helium > Double.Epsilon || !Double.Equals(narcoticOxigenToNitrogen, 1.0))
         {
            double airNarcotic = Utility.airOxigenContent * narcoticOxigenToNitrogen + (1.0 - Utility.airOxigenContent);
            double gasNarcotic = gas.Oxygen * narcoticOxigenToNitrogen + gas.Nitrogen;
            end = DepthAtPresure(airNarcotic / gasNarcotic * PresureAtDepth(depth));
         }
         return end;
      }
      /// <summary>
      /// EDD - Eqivalent Dencity Depth - równoważna głębokość gęstościowa
      /// </summary>
      /// <param name="gas">Mieszanka oddechowa</param>
      /// <param name="depth">Głębokość</param>
      /// <returns>Zwraca głebokość przy której gęstość powietrza byłaby identyczna z gęstością mieszanki dla zadanej głębokości</returns>
      public double EDD(GasMix gas, double depth)
      {
         double edd = depth;
         if (gas.Helium > Double.Epsilon || !Double.Equals(gas.Oxygen, Utility.densityOxygen))
         {
            double airDencity = Utility.airOxigenContent * Utility.densityOxygen + (1.0 - Utility.airOxigenContent) * Utility.densityNitrogen;
            double gasDencity = gas.Oxygen * Utility.densityOxygen + gas.Nitrogen * Utility.densityNitrogen + gas.Helium * Utility.densityHelium;
            edd = DepthAtPresure((airDencity / gasDencity) * PresureAtDepth(depth));
         }
         return edd;
      }
      public bool IsTmxDiving()
      {
         bool tmx = false;
         if(gasesList != null && gasesList.Count()>0)
            tmx = gasesList.Max(g => g.Helium) > Double.Epsilon;
         return tmx;
      }
   }
}
