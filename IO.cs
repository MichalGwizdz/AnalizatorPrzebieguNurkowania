using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;


namespace InputOutput
{
    class IO
    {
        string path = Directory.GetCurrentDirectory();
        public enum InputFileType
        {
            XMLSpreadsheet2003,
            XML,
            CSV,
        }
        public enum DataType
        {
            Gases,
            Profile,
            Compartments,
            All,
        }
        public bool LoadFromFile()
        {
            bool sucess = false;
            return sucess;
        }
        public List<Data.GasMix> LoadGases(string fileName = "")
        {
            List<Data.GasMix> gasesList = new List<Data.GasMix>();
            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(gasesList.GetType());
            StreamReader streamReader = null;
            try
            {
                if (fileName.Length == 0)
                    fileName = path + "\\defaultGases.xml";
                streamReader = new StreamReader(fileName);
                object obj = serializer.Deserialize(streamReader);
                gasesList = (List<Data.GasMix>)obj;
            }
            catch (Exception e)
            {
                gasesList = null;
            }
            finally
            {
                if (null != streamReader)
                {
                    streamReader.Dispose();
                }
                else
                    gasesList = null;
            }
            return gasesList;
        }
        public bool SaveToXML(string path, object objectToSave)
        {
           bool ret = true;
           XmlSerializer serializer = new XmlSerializer(objectToSave.GetType());
           StreamWriter streamWriter = null;
           try
           {
              streamWriter = new StreamWriter(path);
              serializer.Serialize(streamWriter, objectToSave);
           }
           catch (Exception e)
           {
              ret = false;
           }
           finally
           {
              if (null != streamWriter)
              {
                 streamWriter.Dispose();
              }
              else
                 ret = false;
           }
           return ret;
        }
        public bool OpenDivingXML(string path, ref Data.Diving diving)
        {
            bool ret = true;
            XmlSerializer serializer = new XmlSerializer(diving.GetType());
            StreamReader streamReader = null;
            try{
                streamReader = new StreamReader(path);
                diving = (Data.Diving)serializer.Deserialize(streamReader);
            }
            catch (Exception e)
            {
                ret = false;
            }
            finally
            {
                if (null != streamReader)
                {
                    streamReader.Dispose();
                }
                else
                    ret = false;
            }
            return ret;
        }
        public bool SaveToExcel(string filename, DataType type, Data.Diving diving, Compartments.ListOfCompartment compartments, List<Result.PPraeasureForCompartment> result)
        {
            bool ret = true;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XMLSpreadsheet2003Heder(ref sb);
            if (DataType.All == type || DataType.Gases == type)
            {
                addGassesToXMLSpreadsheet2003(ref sb, diving.GasesList);
            }
            if (DataType.All == type || DataType.Profile == type)
            {
                addProfileToXMLSpreadsheet2003(ref sb, diving.SegmentsList);
            }
            if (DataType.All == type || DataType.Compartments == type)
            {
                addCompatmentsToXMLSpreadsheet2003(ref sb, compartments, result);
            }
            XMLSpreadsheet2003Footer(ref sb);
            try
            {
                StreamWriter streamWriter = new StreamWriter(filename);
                streamWriter.Write(sb.ToString());
                if (null != streamWriter)
                {
                    streamWriter.Dispose();
                }
                else
                    ret = false;
            }
            catch (Exception e)
            {
                ret = false;
            }

            return ret;
        }
        private void XMLSpreadsheet2003Heder(ref System.Text.StringBuilder stringBuilder)
        {
            stringBuilder.Append("<?xml version=\"1.0\"?>\n");
            stringBuilder.Append("<?mso-application progid=\"Excel.Sheet\"?>\n");
            stringBuilder.Append("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\">\n");
            stringBuilder.Append("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">\n");
            stringBuilder.Append(" <Author>Michal Gwizdz</Author>\n");
            stringBuilder.Append(" <LastAuthor>AnalizatorNurkownaia</LastAuthor>\n");
            string dataTime = System.DateTime.Today.ToShortDateString() + "T" + System.DateTime.UtcNow.ToLongTimeString() + "Z";
            stringBuilder.Append(" <Created>" + dataTime + "</Created>\n");
            stringBuilder.Append(" <Company>MIG</Company>\n");
            stringBuilder.Append(" <Version>14.00</Version>\n");
            stringBuilder.Append("</DocumentProperties>\n");
        }
        private void XMLSpreadsheet2003Footer(ref System.Text.StringBuilder stringBuilder)
        {
            stringBuilder.Append("</Workbook>\n");
        }
        private void addGassesToXMLSpreadsheet2003(ref System.Text.StringBuilder stringBuilder, List<Data.GasMix> gases)
        {
            stringBuilder.Append("<Worksheet ss:Name=\"" + AnalizatorNurkowaniaWFA.Properties.Resources.gasesMixName + "\">\n");
            stringBuilder.Append(" <Table>\n");
            Data.GasMix trimix = gases.Find( item => item.Helium > Double.Epsilon);
            stringBuilder.Append("  <Row>\n");

            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.gasNameExcel));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.O2NameExcel));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.N2NameExcel));
            if (trimix != null)
            {
                stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.HeNameExcel));
            }
            stringBuilder.Append("  </Row>\n");

            foreach (Data.GasMix gas in gases)
            {
                stringBuilder.Append("  <Row>\n");
                stringBuilder.Append(cellXMLSpreadsheet2003(gas.Name));
                stringBuilder.Append(cellXMLSpreadsheet2003(gas.Oxygen));
                stringBuilder.Append(cellXMLSpreadsheet2003(gas.Nitrogen));
                if (trimix != null)
                {
                    stringBuilder.Append(cellXMLSpreadsheet2003(gas.Name));
                }
                stringBuilder.Append("  </Row>\n");
            }
            stringBuilder.Append(" </Table>\n");
            stringBuilder.Append("</Worksheet>\n");
        }
        private void addProfileToXMLSpreadsheet2003(ref System.Text.StringBuilder stringBuilder, List<Data.DiveSegment> profile)
        {
            stringBuilder.Append("<Worksheet ss:Name=\"" + AnalizatorNurkowaniaWFA.Properties.Resources.profileName + "\">\n");
            stringBuilder.Append(" <Table>\n");

            stringBuilder.Append("  <Row>\n");
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.timeWithUnit));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.initialDepthExcel));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.finialDepthExcel));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.gasNameExcel));
            stringBuilder.Append("  </Row>\n");

            foreach (Data.DiveSegment segment in profile)
            {
                stringBuilder.Append("  <Row>\n");
                stringBuilder.Append(cellXMLSpreadsheet2003(segment.Time));
                stringBuilder.Append(cellXMLSpreadsheet2003(segment.InitialDepth));
                stringBuilder.Append(cellXMLSpreadsheet2003(segment.FinialDepth));
                stringBuilder.Append(cellXMLSpreadsheet2003(segment.GasName));
                stringBuilder.Append("  </Row>\n");
            }
            stringBuilder.Append(" </Table>\n");
            stringBuilder.Append("</Worksheet>\n");
        }
        private void addCompatmentsToXMLSpreadsheet2003(ref System.Text.StringBuilder stringBuilder, Compartments.ListOfCompartment compartments, List<Result.PPraeasureForCompartment> result)
        {
            stringBuilder.Append("<Worksheet ss:Name=\"" + AnalizatorNurkowaniaWFA.Properties.Resources.compartmentName + "\">\n");
            stringBuilder.Append(" <Table>\n");

            stringBuilder.Append("  <Row>\n");
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.timeWithUnit));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.depthWithUnit));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.ambientPreasureExcel));
            stringBuilder.Append(cellXMLSpreadsheet2003(AnalizatorNurkowaniaWFA.Properties.Resources.pPN2Excel));
            foreach (Compartments.Compartment compartment in compartments.compartment)
            {
                stringBuilder.Append(cellXMLSpreadsheet2003(compartment.Name + " HT=" + compartment.HalfTimeN2.ToString("0.0")));
            }
            stringBuilder.Append("  </Row>\n");

            foreach (Result.PPraeasureForCompartment ppCompartment in result)
            {
                stringBuilder.Append("  <Row>\n");
                stringBuilder.Append(cellXMLSpreadsheet2003(ppCompartment.Time));
                stringBuilder.Append(cellXMLSpreadsheet2003(ppCompartment.Depth));
                stringBuilder.Append(cellXMLSpreadsheet2003(ppCompartment.AmbientPreasure));
                stringBuilder.Append(cellXMLSpreadsheet2003(ppCompartment.PreasureN2));
                for (int i = 0; i < compartments.compartment.Count(); i++)
                {
                    stringBuilder.Append(cellXMLSpreadsheet2003(ppCompartment.getN2(i)));
                }
                stringBuilder.Append("  </Row>\n");
            }
            stringBuilder.Append(" </Table>\n");
            stringBuilder.Append("</Worksheet>\n");
        }
        private string cellXMLSpreadsheet2003(object val)
        {
            string cell = "   <Cell>";
            if (val is string)
            {
                cell += "<Data ss:Type=\"String\">" + val.ToString();
            }
            else
            {
                cell += "<Data ss:Type=\"Number\">";
                double d = (double)val;
                cell += d.ToString(new System.Globalization.CultureInfo("en-US"));
            }
            cell += "</Data></Cell>\n";
            return cell;
        }
    }
}
