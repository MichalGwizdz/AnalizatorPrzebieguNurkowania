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
   ///<summary>
   ///Za pomocą tej klasy jest opisana mieszanina gazów
   ///</summary>
   [XmlRoot("GasMix")]
   public class GasMix
   {
      /// <summary>
      /// Nazwa mieszanki, musi być unikalna
      /// </summary>
      [XmlAttribute("Name")]
      public string Name { get; set; }
      /// <summary>
      /// Udział tlenu, jako ułamek
      /// </summary>
      [XmlAttribute("O2")]
      public double Oxygen { get; set; }
      /// <summary>
      /// Udział helu, jako ułamek
      /// </summary>
      [XmlAttribute("He")]
      public double Helium { get; set; }
      /// <summary>
      /// Udział azotu, jako ułamek obliczny na podtswie zawartości helu i tlenu
      /// </summary>
      [XmlIgnore]
      public double Nitrogen
      {
         get { return (1.0 - Helium - Oxygen); }
      }
      /// <summary>
      /// Konstruktor tworzy domyslną mieszaninę - powietrze
      /// </summary>
      public GasMix()
      {
         Name = AnalizatorNurkowaniaWFA.Properties.Resources.Air;
         Oxygen = Utility.airOxigenContent;
         Helium = 0.0;
      }
      /// <summary>
      ///  Konstruktor tworzy mieszaninę podstawie parametrów
      /// </summary>
      /// <remarks>Parametry heliumContent i mixName nie sa obowiązkowe. Domyslnie zostanie nitroks z automatycznie wygenerowana nazwa domyślną</remarks>
      /// <param name="oxigenContent">Zawartość tlenu</param>
      /// <param name="heliumContent">Zawartość helu - opcjonalnie</param>
      /// <param name="mixName">Nazwa mieszanki - opcjonalnie</param>
      public GasMix(double oxigenContent, double heliumContent = 0, string mixName = null)
      {
         Oxygen = oxigenContent;
         Helium = heliumContent;
         if (mixName == null)
            setDefaultName();
         else
            Name = mixName;
         if (!Verify())
            throw new Exception("Invalid gas content");
      }
      /// <summary>
      /// Tworzy nazwę domyślną mieszanki, na podstawie składu
      /// </summary>
      public void setDefaultName()
      {
         if (Helium > Double.Epsilon)
         {
            Name = AnalizatorNurkowaniaWFA.Properties.Resources.TMX;
            int He = (int)Math.Round(100 * Helium);
            int O2 = (int)Math.Round(100 * Oxygen);
            Name += " " + O2.ToString() + "/" + He.ToString();
         }
         else if (Nitrogen > 0.785 && Nitrogen < 0.795)
         {
            Name = AnalizatorNurkowaniaWFA.Properties.Resources.Air;
         }
         else
         {
            Name = AnalizatorNurkowaniaWFA.Properties.Resources.EAN;
            int O2 = (int)Math.Round(100 * Oxygen);
            Name += " " + O2.ToString();
         }

      }
      /// <summary>
      /// Weryfikuje poprawność składu mieszanki
      /// </summary>
      /// <returns>true - jeśli skład jest poprawny, false - suma frakcji gazów > 1.0 </returns>
      public bool Verify()
      {
         bool ret = (Nitrogen > -Double.Epsilon);
         if (ret)
            ret = (Name.Length != 0);
         return ret;
      }
   }
}
