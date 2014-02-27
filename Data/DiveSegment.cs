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

namespace Data
{
   ///<summary>Za pomocą tej klasy jest opisany segment nurkowania</summary>
   ///<remarks>W obrębie segmenty głębokość się nie zmienia lub zmiana jest liniowa, w segmencie moze być uzyty tylko jeden gaz</remarks>
   public class DiveSegment
   {
      ///<summary>Typ służący do identyfikacji rodzaju segmentu</summary>
      public enum segmentType
      {
         ///<summary>Segment utworzony przez użytkownika</summary>
         user,
         ///<summary>Segment wygenerowny przez program - tak możemy oznaczyć przystanki dekompresyjne</summary>
         program,
      }
      ///<summary>
      ///Nazwa segmentu, służy do identyfikacji, musi być unikalna
      ///</summary>
      [XmlAttribute("Name")]
      public string Name { get; set; }

      ///<summary>
      ///Głebokość poczatkowa segmentu [m]
      ///</summary>
      [XmlAttribute("InitialDepth")]
      public double InitialDepth { get; set; }

      //<summary>
      ///Głebokość końcowa segmentu [m]
      ///</summary>
      [XmlAttribute("FinialDepth")]
      public double FinialDepth { get; set; }

      ///<summary>
      ///Czas trwania segmentu [min]
      ///</summary>
      [XmlAttribute("Time")]
      public double Time { get; set; }

      ///<summary>
      ///Nazwa mieszanki gazów wykorzystywej w danym segmencie
      ///</summary>
      [XmlAttribute("GasName")]
      public string GasName { get; set; }

      ///<summary>
      ///Identyfikator pozwala stwierdzić czy segment został wprowadzony przez użytkownika
      ///</summary>
      [XmlAttribute("Type")]
      public segmentType Type { get; set; }

      ///<summary>Tworzy domyślny segment</summary>
      public DiveSegment()
      {
         InitialDepth = 0;
         FinialDepth = 0;
         Time = 0;
         GasName = AnalizatorNurkowaniaWFA.Properties.Resources.Air;
         Type = segmentType.program;
         Name = "Default";
      }
      /// <summary>
      /// Tworzy segment nurkowania.
      /// </summary>
      /// <remarks>Parametry segmentType i segmentName nie sa obowiązkowe. Domyslnie zostanie utworzony segment "uzytkownika" z automatycznie wygenerowana nazwa domyślną</remarks>
      /// <param name="initialDepth">Głębokość poczatkowa</param>
      /// <param name="finialDepth">Głębokość końcowa</param>
      /// <param name="segmentTime">Czas trwania</param>
      /// <param name="segmentGasName">Nazwa mieszanki</param>
      /// <param name="segmentType">Typ segmentu</param>
      /// <param name="segmentName">Nazwa segmentu</param>
      public DiveSegment(double initialDepth, double finialDepth, double segmentTime, string segmentGasName, segmentType segmentType = segmentType.user, string segmentName = null)
      {
         InitialDepth = initialDepth;
         FinialDepth = finialDepth;
         Time = segmentTime;
         GasName = segmentGasName;
         Type = segmentType;
         if (segmentName == null)
            setDefaultName();
         else
            Name = segmentName;
         if (!Verify())
            throw new Exception("Invalid parameters");
      }
      /// <summary>
      /// Generuje nazwę domyślną segmentu. Nazwa zawiera głebokość początkową i końcową, czas oraz nazwę użytego gazu
      /// </summary>
      public void setDefaultName()
      {
         Name = "d=" + InitialDepth.ToString("0") + "-" + FinialDepth.ToString("0") + " [m] t=" + Time.ToString("0.0") + "[min] " + GasName;
      }
      /// <summary>
      /// Sprawdza poprawność segmentu.
      /// </summary>
      /// <returns>true - jesli segment jest poprawny, false - gdy parametry zawierają błąd</returns>
      public bool Verify()
      {
         bool ret = (Time > -Double.Epsilon && InitialDepth > -Double.Epsilon && FinialDepth > -Double.Epsilon && GasName.Length > 0);
         if (ret)
            ret = (Name.Length > 0);
         return ret;
      }
   }
}
