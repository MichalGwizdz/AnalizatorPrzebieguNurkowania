using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnalizatorNurkowaniaWFA
{
    public partial class FormParams : Form 
    {
       private bool withUpdate;
       public enum tabs
       {
          general,
          deco,
          dive,
          parameters,

       }
        public FormParams()
        {
            InitializeComponent();
            withUpdate = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            withUpdate = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            withUpdate = false; 
        }
        public void SetActiveTab(tabs activeTab)
        {
           switch (activeTab)
           {
              case tabs.general:
                 this.tabParGeneral.Select();
                 break;
              case tabs.deco:
                 this.tabParDeco.Select();
                 break;
              case tabs.dive:
                 this.tabParDive.Select();
                 break;
              case tabs.parameters:
                 this.tabParameters.Select();
                 break;
           }
        }
        public bool UpdateUserData(ref userProperties userProp)
        {
           withUpdate = false;
           predkoscZanurzania.Text = userProp.AscentSpeed.ToString("0.0");
           predkoscWynurzania.Text = userProp.DescentSpeed.ToString("0.0");
           cisnienieAtmosferyczne.Text = userProp.AtmosphericPressure.ToString("0.0");
           ciezarWody.Text = userProp.WaterSpecificWeight.ToString("0.0");
           switch (userProp.compartmentType)
           {
               case Compartments.CompartmentsType.ZHL16C:
                   radioZHL16C.Checked = true;
                   break;
               case Compartments.CompartmentsType.ZHL16B:
                   radioZHL16B.Checked = true;
                   break;
               case Compartments.CompartmentsType.ZHL16A:
                   radioZHL16A.Checked =true;
                   break;   
               case Compartments.CompartmentsType.DCAP_MF11F6:
                   radioMF11F6.Checked = true;
                   break;
               case Compartments.CompartmentsType.ZHL12:
                   radioZHL12.Checked = true;
                   break;
               case Compartments.CompartmentsType.Workman:
                   radioWorkman.Checked = true;
                   break;
           }

           this.ShowDialog();
           if (withUpdate)
           {
            userProp.AscentSpeed = Double.Parse(predkoscZanurzania.Text);
            userProp.DescentSpeed= Double.Parse(predkoscWynurzania.Text);
            userProp.AtmosphericPressure = Double.Parse(cisnienieAtmosferyczne.Text);
            userProp.WaterSpecificWeight = Double.Parse(ciezarWody.Text);
            if (radioZHL16C.Checked)
                userProp.compartmentType = Compartments.CompartmentsType.ZHL16C;
            else if (radioZHL16B.Checked)
                userProp.compartmentType = Compartments.CompartmentsType.ZHL16B;
            else if (radioZHL16A.Checked)
                userProp.compartmentType = Compartments.CompartmentsType.ZHL16A;
            else if (radioMF11F6.Checked)
                userProp.compartmentType = Compartments.CompartmentsType.DCAP_MF11F6;
            else if (radioZHL12.Checked)
                userProp.compartmentType = Compartments.CompartmentsType.ZHL12;
            else if (radioWorkman.Checked)
                userProp.compartmentType = Compartments.CompartmentsType.Workman;
           }
           return withUpdate;

        }
    }
}
