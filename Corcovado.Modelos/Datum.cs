using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corcovado.Modelos
{
    public class Datum
    {
        public double _a { get; set; }
        public double _b { get; set; }

        public int SINAL(double num)
        {
            if (num == 0)
            {
                return 0;
            }
            else if (num > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public int INT(double num)
        {
            return (int) Math.Floor(num);
        }

        public double RAIZ(double num)
        {
           return Math.Sqrt(num);
        }

        public double RADIANOS(double num) // converte graus para radianos
        {
            return num * (Math.PI / 180.0);
        }


        public double GRAUS(double num) // converte graus para radianos
        {
            return num *(180.0 / Math.PI);
        }

        public double SEN(double num)
        {
            return Math.Sin(num);
        }

        public double COS(double num)
        {
            return Math.Cos(num);
        }

        public double ACOS(double num)
        {
            return Math.Acos(num);
        }

        public double TAN(double num)
        {
            return Math.Tan(num);
        }

        public double PI()
        {
            return Math.PI;
        }

        public double lat_a { get; set; }
        public double lat_b { get; set; }
        public double lon_a { get; set; }
        public double lon_b { get; set; }

        #region FM
        public double fm_decimal { get; set; }
        public double fm_grau { get; set; }
        public double fm_minuto { get; set; }
        public double fm_segundo { get; set; }

        public void CalculoFmDecimal()
        {
            fm_decimal = (lat_a + lat_b) / 2;
        }

        public void CalculoFmGrau()
        {
            this.fm_grau = (INT(fm_decimal * SINAL(fm_decimal)) * SINAL(fm_decimal));
        }


        public void CalculoFmMinuto()
        {
            this.fm_minuto = INT((fm_decimal * SINAL(fm_decimal) - (INT(fm_decimal * SINAL(fm_decimal)))) * 60);

        }

        public void CalculoFmSegundo()
        {
            this.fm_segundo = ((((fm_decimal * SINAL(fm_decimal) - (INT(fm_decimal * SINAL(fm_decimal)))) * 60)) - INT((((fm_decimal * SINAL(fm_decimal) - (INT(fm_decimal * SINAL(fm_decimal)))) * 60)))) * 60;
        }
        #endregion

        #region DF
        public double df_segundo { get; set; }

        public void CalculoDfSegundo()
        {
            df_segundo = (lat_a - lat_b) * 3600;
        }
        #endregion

        #region DL
        public double dl_segundo  { get; set; }

        public void CalculoDlSegundo() 
        {
            dl_segundo = (lon_a - lon_b) * 3600;
        }
        #endregion

        #region NA
        public double NA { get; set; }
        public void CalculoNA()
        {
            NA = _a / (RAIZ((1 - ((Math.Pow(_a, 2) - Math.Pow(_b, 2)) / Math.Pow(_a, 2)) * Math.Pow(SEN(RADIANOS(lat_a)), 2))));
        }
        #endregion

        #region MA
        public double MA { get; set; }
        public void CalculoMA()
        {
            MA = (_a * (1 - ((Math.Pow(_a, 2) - Math.Pow(_b, 2)) / Math.Pow(_a, 2))))/ 
                (Math.Pow(((1 - ((Math.Pow(_a, 2) - Math.Pow(_b, 2))/ Math.Pow(_a, 2))* Math.Pow(SEN(RADIANOS(lat_a)) , 2)   )), (3.0 / 2.0) ) );
        }
        #endregion

        #region NB
        public double NB { get; set; }

        public void CalculoNB()
        {
            NB = _a / (RAIZ((1 - ((Math.Pow(_a, 2) - Math.Pow(_b, 2)) / Math.Pow(_a, 2)) * Math.Pow(SEN(RADIANOS(lat_b)), 2))));
        }
        #endregion

        #region X1
        public double X1 { get; set; }
        public void CalculoX1()
        {
            X1 = (PI() * NB * COS(RADIANOS(lat_b)) * (dl_segundo / 3600)) / 180.0;
        }
        #endregion

        #region Y1
        public double Y1 { get; set; }
        public void CalculoY1()
        {
            Y1 = (PI() * MA * (df_segundo / 3600)) / 180.0;
        }
        #endregion

        #region DE1
        public double DE1 { get; set; }
        public void CalculoDE1()
        {
            DE1 = RAIZ(Math.Pow( X1 , 2) +  Math.Pow( Y1 , 2));
        }
        #endregion

        #region AZ
        public double az_decimal { get; set; }
        public double az_grau { get; set; }
        public double az_minuto { get; set; }
        public double az_segundo { get; set; }

        public void CalculoAzDecimal()
        {
            if (X1<0)
            {
                az_decimal = 360 - (GRAUS(ACOS(Y1 / DE1)));
            }
            else
            {
                az_decimal = (GRAUS(ACOS(Y1 / DE1)));
            }
        }

        public void CalculoAzGrau()
        {
            az_grau = (INT(az_decimal * SINAL(az_decimal)) * SINAL(az_decimal));
        }

        public void CalculoAzMinuto()
        {
            az_minuto = INT((az_decimal * SINAL(az_decimal) - (INT(az_decimal * SINAL(az_decimal)))) * 60);
        }

        public void CalculoAzSegundo()
        {
            az_segundo =  ((((az_decimal * SINAL(az_decimal) - (INT(az_decimal * SINAL(az_decimal)))) * 60)) - INT((((az_decimal * SINAL(az_decimal) - (INT(az_decimal * SINAL(az_decimal)))) * 60)))) * 60;
        }
        #endregion

        #region B
        public double B { get; set; }
        public void CalculoB()
        {
            B = 1 / (MA * 4.84813681107636E-06);
        }
        #endregion

        #region C
        public double C { get; set; }
        public void CalculoC()
        {
            C = TAN(RADIANOS(lat_a)) / (2 * NA * MA * 4.84813681107636E-06);
        }
        #endregion

    }
}
