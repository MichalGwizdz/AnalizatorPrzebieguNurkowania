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
    public class ProfileData
    {
        public struct profileData
        {
            public double initialT;
            public double deltaT;
            public double initialD;
            public double deltaD;
            public double initialP;
            public double deltaP;
            public double initialPPN2;
            public double deltaPPN2;
            public double initialPPHe;
            public double deltaPPHe;
            public double initialPPO2;
            public double deltaPPO2;
        }
        private List<profileData> ListOfProfileData;
        public ProfileData()
        {
            ListOfProfileData = new List<profileData>();
        }
        public void Add(profileData newData)
        {
            ListOfProfileData.Add(newData);
        }
        public void Reset()
        {
            ListOfProfileData.RemoveRange(0, ListOfProfileData.Count());
        }
        public double getPartialPreasureN2(double time, int segmentNo)
        {
            return ListOfProfileData[segmentNo].initialPPN2 + (time - ListOfProfileData[segmentNo].initialT)/ListOfProfileData[segmentNo].deltaT *ListOfProfileData[segmentNo].deltaPPN2;
        }
        public double getPartialPreasurO2(double time, int segmentNo)
        {
            return ListOfProfileData[segmentNo].initialPPO2 + (time - ListOfProfileData[segmentNo].initialT) / ListOfProfileData[segmentNo].deltaT * ListOfProfileData[segmentNo].deltaPPO2;
        }
        public double getPartialPreasureHe(double time, int segmentNo)
        {
            return ListOfProfileData[segmentNo].initialPPHe + (time - ListOfProfileData[segmentNo].initialT) / ListOfProfileData[segmentNo].deltaT * ListOfProfileData[segmentNo].deltaPPHe;
        }
        public double getPreasure(double time, int segmentNo)
        {
            return ListOfProfileData[segmentNo].initialP + (time - ListOfProfileData[segmentNo].initialT) / ListOfProfileData[segmentNo].deltaT * ListOfProfileData[segmentNo].deltaP;
        }
        public double getDepth(double time, int segmentNo)
        {
            return ListOfProfileData[segmentNo].initialD + (time - ListOfProfileData[segmentNo].initialT) / ListOfProfileData[segmentNo].deltaT * ListOfProfileData[segmentNo].deltaD;
        }
        public int Count()
        {
            return ListOfProfileData.Count();
        }
        public double getInitTime(int segmentNo)
        {
            return ListOfProfileData[segmentNo].initialT;
        }
        public double getTime(int segmentNo)
        {
            return ListOfProfileData[segmentNo].deltaT;
        }
        public int getSegmentNo (double time)
        {
            int count = ListOfProfileData.Count();
            int i = count-1;
            for ( ; i >=0 ; i--)
            {
                if (ListOfProfileData[i].initialT <= time)
                {
                    break;
                }
            }
            return i;
        }
    }
}

