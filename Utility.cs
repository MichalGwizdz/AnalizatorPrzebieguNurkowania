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

namespace Tools
{
   ///<summary>Miejsce przeznaczone na stałe oraz funkcje narzędziowe</summary>
   public struct Utility
   {
      /// <summary>
      /// Stała: przyśpieszenie ziemskie
      /// </summary>
      public const double gravity = 9.80665;
      /// <summary>
      /// Gęstość helu dla 0*C
      /// </summary>
      public const double densityHelium = 0.1786;
      /// <summary>
      /// Gęstość tlenu dla 0*C
      /// </summary>
      public const double densityOxygen = 1.429;
      /// <summary>
      /// Gęstość azotu dla 0*C
      /// </summary>
      public const double densityNitrogen = 1.251;
      /// <summary>
      /// Zawartość tlenu w powietrzu
      /// </summary>
      public const double airOxigenContent = 0.21;
      /// <summary>
      /// Zwraca czas w minutach. Dokładnosć 1 [sek]. Może zaokrąglać w dół lub w górę do pełnych minut
      /// </summary>
      /// <param name="h">Godziny</param>
      /// <param name="m">Minuty</param>
      /// <param name="s">Sekundy</param>
      /// <param name="roundToNearestMinut">0 - dokładnosć 1 [sek], -1 do mniejszej pełnej minuty, +1 do wiekszej pełnej minuty</param>
      /// <returns></returns>
      public double convertToMinutes(double h, double m, double s, int roundToNearestMinut = 0)
      {
         s += h * 3600.0;
         s = (double)Math.Ceiling(s);
         double min = m + s / 60.0;
         if (roundToNearestMinut > 0)
            min = (double)Math.Floor(min);
         else if (roundToNearestMinut < 0)
            min = (double)Math.Ceiling(min);
         return min;
      }
      public double convertToMinutes(string time)
      {
         double min = 0;
         string[] val = time.Split(':');
         int valNo = val.Length;
         if (valNo < 3)
         {
            try
            {
               switch (val.Length)
               {
                  case 1:
                     min = int.Parse(val[0]);
                     break;
                  case 2:
                     min = int.Parse(val[0])*60 + int.Parse(val[1]);
                     break;
                  case 3:
                     min = int.Parse(val[0]) * 60 + int.Parse(val[1]) + (int)Math.Round(double.Parse(val[2])/60.0);
                     break;
               }
            }
            catch (Exception e)
            {
            }
         }
         return min;
      }
   }
}
