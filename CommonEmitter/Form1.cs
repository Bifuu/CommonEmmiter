using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonEmitter
{
    public partial class Form1 : Form
    {
        bool isPNP = false;
        enum configure { Emitter, Base, Collector };
        configure config = configure.Emitter;

        #region Vars
        double vcc;
        double ic;
        double beta;

        double rc;
        double re;

        double ie;
        double ib;
        double vRc;
        double vCE;
        double vRe;
        double vBe;
        double r1;
        double r2;
        double vB;
        double vR2;
        double vR1;

        double rePrime;
        double zin;
        double zinQ;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Calculate();
            WriteToFile();
        }

        private void WriteToFile()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("history.txt");
            file.WriteLine("INPUTS: Type:" + comboBox1.Text + " Common-" + comboBox2.Text + " VCC: " + txtVCC.Text + " Ic: " + txtIc.Text + " Beta: " + txtBeta.Text + "\n\tOUTPUTS: " + "V1: " + txtVr1.Text + " I1: " + txtI1.Text + " R1: " + txtR1.Text + " Vr2: " + txtVr2.Text + " I2: " + txtI2.Text + " R2: " + txtR2.Text + " Vb: " + txtVb.Text + " Ib: " + txtIb.Text + " Rc: " + txtRc.Text + " Vc: " + txtVc.Text + " Re: " + txtRe.Text + " Ve: " + txtVe.Text + " Ie: " + txtIe.Text + " Gain: " + txtGain.Text + " Zin(Q): " + txtZinq.Text + " Zin: " + txtZin.Text);
            file.Close();
        }

        public void Calculate()
        {

            #region NPN emitter DONE
            //Given: VCC, IC, Beta
            //Vrc = .45 x vcc, Vce = .45 x vcc, Vre = .1 x vcc
            if ((config == configure.Emitter) && (!isPNP))
            {
                vcc = Convert.ToDouble(txtVCC.Text);
                ic = Convert.ToDouble(txtIc.Text);
                beta = Convert.ToDouble(txtBeta.Text);

                vBe = 0.7;

                vRc = vcc * 0.45;
                vCE = vcc * 0.45;
                vRe = vcc * 0.10;

                rc = vRc / ic;
                ib = ic / beta;
                ie = ic + ib;
                re = vRe / ie;
                vB = vRe + vBe;

                vR2 = vB;
                r2 = vR2 / (8 * ib);
                vR1 = vcc - vR2;
                r1 = (vcc - vB) / (9 * ib);

                rePrime = CalcRePrime(ie);

                txtI1.Text = String.Format("{0:F9}", (9 * ib));
                txtI2.Text = String.Format("{0:F9}", (8 * ib));
                txtIb.Text = String.Format("{0:F9}", ib);
                txtIe.Text = String.Format("{0:F9}", ie);
                txtR1.Text = String.Format("{0:F2}", r1);
                txtR2.Text = String.Format("{0:F2}", r2);
                txtRc.Text = String.Format("{0:F2}", rc);
                txtRe.Text = String.Format("{0:F2}", re);
                txtVb.Text = String.Format("{0:F2}", vB);
                txtVc.Text = String.Format("{0:F2}", vRc);
                txtVe.Text = String.Format("{0:F2}", vRe);
                txtVr1.Text = String.Format("{0:F2}", vR1);
                txtVr2.Text = String.Format("{0:F2}", vR2);
                txtGain.Text = String.Format("{0:F2}", ((-1) * rc / rePrime));
                txtZinq.Text = String.Format("{0:F2}", (beta * rePrime));
                txtZin.Text = String.Format("{0:F9}", (1 / ((1 / (beta * rePrime)) + (1 / r1) + (1 / r2))));
            }
            #endregion

            #region NPN base DONE
            //Given: VCC, IC, Beta
            //Vrc = .45 x vcc, Vce = .45 x vcc, Vre = .1 x vcc
            if ((config == configure.Base) && (!isPNP))
            {
                vcc = Convert.ToDouble(txtVCC.Text);
                ic = Convert.ToDouble(txtIc.Text);
                beta = Convert.ToDouble(txtBeta.Text);

                vBe = 0.7;

                vRc = vcc * 0.45;
                vCE = vcc * 0.45;
                vRe = vcc * 0.10;

                rc = vRc / ic;
                ib = ic / beta;
                ie = ic + ib;
                re = vRe / ie;
                vB = vRe + vBe;

                vR2 = vB;
                r2 = vR2 / (8 * ib);
                vR1 = vcc - vR2;
                r1 = (vcc - vB) / (9 * ib);

                rePrime = CalcRePrime(ie);

                txtI1.Text = String.Format("{0:F9}", (9 * ib));
                txtI2.Text = String.Format("{0:F9}", (8 * ib));
                txtIb.Text = String.Format("{0:F9}", ib);
                txtIe.Text = String.Format("{0:F9}", ie);
                txtR1.Text = String.Format("{0:F2}", r1);
                txtR2.Text = String.Format("{0:F2}", r2);
                txtRc.Text = String.Format("{0:F2}", rc);
                txtRe.Text = String.Format("{0:F2}", re);
                txtVb.Text = String.Format("{0:F2}", vB);
                txtVc.Text = String.Format("{0:F2}", vRc);
                txtVe.Text = String.Format("{0:F2}", vRe);
                txtVr1.Text = String.Format("{0:F2}", vR1);
                txtVr2.Text = String.Format("{0:F2}", vR2);
                txtGain.Text = String.Format("{0:F2}", (rc / rePrime));
                txtZinq.Text = String.Format("{0:F2}", rePrime);
                txtZin.Text = String.Format("{0:F2}", ((rePrime * re) / (re + rePrime)));
            }
            #endregion

            #region NPN Collector DONE
            //Given: VCC, IC, Beta
            //Vrc = 0 x vcc, Vce = .5 x vcc, Vre = .5 x vcc
            if ((config == configure.Collector) && (!isPNP))
            {
                vcc = Convert.ToDouble(txtVCC.Text);
                ic = Convert.ToDouble(txtIc.Text);
                beta = Convert.ToDouble(txtBeta.Text);

                vBe = 0.7;

                vRc = vcc;
                vCE = vcc * 0.50;
                vRe = vcc * 0.50;

                rc = vRc / ic;
                ib = ic / beta;
                ie = ic + ib;
                re = vRe / ie;
                vB = vRe + vBe;

                vR2 = vB;
                r2 = vR2 / (8 * ib);
                vR1 = vcc - vR2;
                r1 = (vcc - vB) / (9 * ib);

                rePrime = CalcRePrime(ie);

                txtI1.Text = String.Format("{0:F9}", (9 * ib));
                txtI2.Text = String.Format("{0:F9}", (8 * ib));
                txtIb.Text = String.Format("{0:F9}", ib);
                txtIe.Text = String.Format("{0:F9}", ie);
                txtR1.Text = String.Format("{0:F2}", r1);
                txtR2.Text = String.Format("{0:F2}", r2);
                txtRc.Text = String.Format("{0:F2}", rc);
                txtRe.Text = String.Format("{0:F2}", re);
                txtVb.Text = String.Format("{0:F2}", vB);
                txtVc.Text = String.Format("{0:F2}", vRc);
                txtVe.Text = String.Format("{0:F2}", vRe);
                txtVr1.Text = String.Format("{0:F2}", vR1);
                txtVr2.Text = String.Format("{0:F2}", vR2);
                txtGain.Text = String.Format("~1");
                txtZinq.Text = String.Format("{0:F2}", (rePrime + ( 1 / (beta + 1)) * ((r1 * r2) / (r1 + r2))));
                txtZin.Text = String.Format("{0:F2}", ((rePrime * re) / (re + rePrime)));
            }
            #endregion

            #region PNP emitter
            //Given: VCC, IC, Beta
            //Vrc = .45 x vcc, Vce = .45 x vcc, Vre = .1 x vcc
            if ((config == configure.Emitter) && (isPNP))
            {
                vcc = Convert.ToDouble(txtVCC.Text);
                ic = Convert.ToDouble(txtIc.Text);
                beta = Convert.ToDouble(txtBeta.Text);

                vBe = 0.7;

                vRc = vcc * 0.45;
                vCE = vcc * 0.45;
                vRe = vcc * 0.10;

                rc = vRc / ic;
                ib = ic / beta;
                ie = ic + ib;
                re = vRe / ie;
                vB = vRe + vBe;

                vR2 = vB;
                r2 = vR2 / (8 * ib);
                vR1 = vcc - vR2;
                r1 = (vcc - vB) / (9 * ib);

                rePrime = CalcRePrime(ie);

                txtI1.Text = String.Format("{0:F9}", (9 * ib));
                txtI2.Text = String.Format("{0:F9}", (8 * ib));
                txtIb.Text = String.Format("{0:F9}", ib);
                txtIe.Text = String.Format("{0:F9}", ie);
                txtR1.Text = String.Format("{0:F2}", r1);
                txtR2.Text = String.Format("{0:F2}", r2);
                txtRc.Text = String.Format("{0:F2}", rc);
                txtRe.Text = String.Format("{0:F2}", re);
                txtVb.Text = String.Format("{0:F2}", vB);
                txtVc.Text = String.Format("{0:F2}", vRc);
                txtVe.Text = String.Format("{0:F2}", vRe);
                txtVr1.Text = String.Format("{0:F2}", vR1);
                txtVr2.Text = String.Format("{0:F2}", vR2);
                txtGain.Text = String.Format("{0:F2}", ((-1) * rc / rePrime));
                txtZinq.Text = String.Format("{0:F2}", (beta * rePrime));
                txtZin.Text = String.Format("{0:F9}", (1 / ((1 / (beta * rePrime)) + (1 / r1) + (1 / r2))));
            }
            #endregion

            #region PNP base
            //Given: VCC, IC, Beta
            //Vrc = .45 x vcc, Vce = .45 x vcc, Vre = .1 x vcc
            if ((config == configure.Base) && (isPNP))
            {
                vcc = Convert.ToDouble(txtVCC.Text);
                ic = Convert.ToDouble(txtIc.Text);
                beta = Convert.ToDouble(txtBeta.Text);

                vBe = 0.7;

                vRc = vcc * 0.45;
                vCE = vcc * 0.45;
                vRe = vcc * 0.10;

                rc = vRc / ic;
                ib = ic / beta;
                ie = ic + ib;
                re = vRe / ie;
                vB = vRe + vBe;

                vR2 = vB;
                r2 = vR2 / (8 * ib);
                vR1 = vcc - vR2;
                r1 = (vcc - vB) / (9 * ib);

                rePrime = CalcRePrime(ie);

                txtI1.Text = String.Format("{0:F9}", (9 * ib));
                txtI2.Text = String.Format("{0:F9}", (8 * ib));
                txtIb.Text = String.Format("{0:F9}", ib);
                txtIe.Text = String.Format("{0:F9}", ie);
                txtR1.Text = String.Format("{0:F2}", r1);
                txtR2.Text = String.Format("{0:F2}", r2);
                txtRc.Text = String.Format("{0:F2}", rc);
                txtRe.Text = String.Format("{0:F2}", re);
                txtVb.Text = String.Format("{0:F2}", vB);
                txtVc.Text = String.Format("{0:F2}", vRc);
                txtVe.Text = String.Format("{0:F2}", vRe);
                txtVr1.Text = String.Format("{0:F2}", vR1);
                txtVr2.Text = String.Format("{0:F2}", vR2);
                txtGain.Text = String.Format("{0:F2}", (rc / rePrime));
                txtZinq.Text = String.Format("{0:F2}", rePrime);
                txtZin.Text = String.Format("{0:F2}", ((rePrime * re) / (re + rePrime)));
            }
            #endregion

            #region PNP Collector
            //Given: VCC, IC, Beta
            //Vrc = 0 x vcc, Vce = .5 x vcc, Vre = .5 x vcc
            if ((config == configure.Collector) && (isPNP))
            {
                vcc = Convert.ToDouble(txtVCC.Text);
                ic = Convert.ToDouble(txtIc.Text);
                beta = Convert.ToDouble(txtBeta.Text);

                vBe = 0.7;

                vRc = vcc;
                vCE = vcc * 0.50;
                vRe = vcc * 0.50;

                rc = vRc / ic;
                ib = ic / beta;
                ie = ic + ib;
                re = vRe / ie;
                vB = vRe + vBe;

                vR2 = vB;
                r2 = vR2 / (8 * ib);
                vR1 = vcc - vR2;
                r1 = (vcc - vB) / (9 * ib);

                rePrime = CalcRePrime(ie);

                txtI1.Text = String.Format("{0:F9}", (9 * ib));
                txtI2.Text = String.Format("{0:F9}", (8 * ib));
                txtIb.Text = String.Format("{0:F9}", ib);
                txtIe.Text = String.Format("{0:F9}", ie);
                txtR1.Text = String.Format("{0:F2}", r1);
                txtR2.Text = String.Format("{0:F2}", r2);
                txtRc.Text = String.Format("{0:F2}", rc);
                txtRe.Text = String.Format("{0:F2}", re);
                txtVb.Text = String.Format("{0:F2}", vB);
                txtVc.Text = String.Format("{0:F2}", vRc);
                txtVe.Text = String.Format("{0:F2}", vRe);
                txtVr1.Text = String.Format("{0:F2}", vR1);
                txtVr2.Text = String.Format("{0:F2}", vR2);
                txtGain.Text = String.Format("~1");
                txtZinq.Text = String.Format("{0:F2}", (rePrime + (1 / (beta + 1)) * ((r1 * r2) / (r1 + r2))));
                txtZin.Text = String.Format("{0:F2}", ((rePrime * re) / (re + rePrime)));
            }
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Solves transistor circuits for various configurations."
                           +"\r\nRequired variables: VCC, IC, Beta "
                           +"\r\nEnter the Values in their boxes then hit Calculate! ", "Help", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePicture();

        }

        public void UpdatePicture()
        {
            if (comboBox1.Text == "NPN")
                isPNP = false;
            else if (comboBox1.Text == "PNP")
                isPNP = true;

            switch (comboBox2.Text)
            {
                case "Emitter":
                    config = configure.Emitter;
                    if (isPNP)
                        pictureBox1.Image = CommonEmitter.Properties.Resources.ampPNPCE;
                    else if (!isPNP)
                        pictureBox1.Image = CommonEmitter.Properties.Resources.ampNPNCE;
                    txtRc.Show();
                    lblRc.Show();
                    break;
                case "Base":
                    config = configure.Base;
                    if (isPNP)
                        pictureBox1.Image = CommonEmitter.Properties.Resources.ampPNPCB;
                    else if (!isPNP)
                        pictureBox1.Image = CommonEmitter.Properties.Resources.ampNPNCB;
                    txtRc.Show();
                    lblRc.Show();
                    break;
                case "Collector":
                    config = configure.Collector;
                    if (isPNP)
                        pictureBox1.Image = CommonEmitter.Properties.Resources.ampPNPCC;
                    else if (!isPNP)
                        pictureBox1.Image = CommonEmitter.Properties.Resources.ampNPNCC;
                    txtRc.Hide();
                    lblRc.Hide();
                    break;
            }
        }

        public double CalcRePrime(double Ie)
        {
            return .025 / Ie;
        }
    }
}
