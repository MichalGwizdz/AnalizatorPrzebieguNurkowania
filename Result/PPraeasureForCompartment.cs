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

namespace Result
{
   /// <summary>
   /// Klasa przetrzymuje rezulatay obliczeń
   /// </summary>
   public class PPraeasureForCompartment
   {
      /// <summary>
      /// Tablica ciśnień dla azotu
      /// </summary>
      private double[] gN2;
      /// <summary>
      /// Tablica ciśnień dla helu
      /// </summary>
      private double[] gHe;
      /// <summary>
      /// Tablica ciśnień dla azotu
      /// </summary>
      private double[] ceiling;
      /// <summary>
      /// Ciśnienie zewnętrzne
      /// </summary>
      public double AmbientPreasure { get; set; }
      /// <summary>
      /// Cisnienie azotu
      /// </summary>
      public double PreasureN2 { get; set; }
      public double PreasureHe { get; set; }
      public double Time { get; set; }
      public double Depth { get; set; }
      public PPraeasureForCompartment(int compartmentNo, bool tmx = false)
      {
         ceiling = new double[compartmentNo];
         gN2 = new double[compartmentNo];
         if (tmx)
            gHe = new double[compartmentNo];
      }
      public double getN2(int compartmentIndex)
      {
         return gN2[compartmentIndex];
      }
      public void setN2(int compartmentIndex, double value)
      {
         gN2[compartmentIndex] = value;
      }
      public double getHe(int compartmentIndex)
      {
         return gHe[compartmentIndex];
      }
      public void setHe(int compartmentIndex, double value)
      {
         gHe[compartmentIndex] = value;
      }
      public double getCeiling(int compartmentIndex)
      {
         return ceiling[compartmentIndex];
      }
      public void setCeiling(int compartmentIndex, double value)
      {
         ceiling[compartmentIndex] = value;
      }
      public int Count()
      {
         return gN2.Length;
      }
   }
}
