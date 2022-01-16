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


        public double lat_a { get; set; }
        public double lat_b { get; set; }
        public double lon_a { get; set; }
        public double lon_b { get; set; }


        public DateTime data_a { get; set; }
        public DateTime data_b { get; set; }

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

        #region D
        public double D { get; set; }
        public void CalculoD()
        {
            D = ((3 * ((Math.Pow(_a, 2) - Math.Pow(_b, 2)) / Math.Pow(_a, 2))) * SEN(RADIANOS(lat_a)) * COS(RADIANOS(lat_a)) * 4.84813681107636E-06) / (2 * Math.Pow(((1 - ((Math.Pow(_a, 2) - Math.Pow(_b, 2)) / Math.Pow(_a, 2)) * Math.Pow(SEN(RADIANOS(lat_a)), 2))) ,(3 / 2)));
        }
        #endregion

        #region E
        public double E { get; set; }
        public void CalculoE()
        {
            E = (1 + 3 * Math.Pow(TAN(RADIANOS(lat_a)) , 2)) / (6 * Math.Pow(NA , 2));
        }
        #endregion

        #region K1
        public double K1 { get; set; }
        public void CalculoK1()
        {
            K1 = B * Y1;
        }
        #endregion

        #region K2
        public double K2 { get; set; }
        public void CalculoK2()
        {
            K2 = C * Math.Pow(X1 , 2);
        }
        #endregion

        #region K3
        public double K3 { get; set; }
        public void CalculoK3()
        {
            K3 = D * Math.Pow(K1, 2);
        }
        #endregion

        #region K4
        public double K4 { get; set; }
        public void CalculoK4()
        {
            K4 = E * K1 * Math.Pow(X1, 2);
        }
        #endregion


        #region K5
        public double K5 { get; set; }
        public void CalculoK5()
        {
            K5 = df_segundo - K2 -K3 + Math.Pow(K4, 2);
        }
        #endregion


        #region Y
        public double Y { get; set; }
        public void CalculoY()
        {
            Y =K5 / B;
        }
        #endregion


        #region C1
        public double C1 { get; set; }
        public void CalculoC1()
        {
            C1 = (Math.Pow((C / B) , 2)) * (Math.Pow(X1 , 3)) * (2 / 3);
        }
        #endregion

        #region C2
        public double C2 { get; set; }
        public void CalculoC2()
        {
            C2 = 0.0000000000000004095 * X1 * Math.Pow(Y1 ,2);
        }
        #endregion

        #region A1B
        public double A1B { get; set; }
        public void CalculoA1B()
        {
            A1B = 1 / (NB * 4.84813681107636E-06 * COS(RADIANOS(lat_b)));
        }
        #endregion

        #region W
        public double W { get; set; }
        public void CalculoW()
        {
            W = dl_segundo / A1B;
        }
        #endregion

        #region X
        public double X { get; set; }
        public void CalculoX()
        {
            X = W - C1 - C2;
        }
        #endregion

        #region distancia_m
        public double distancia_m { get; set; }
        public void CalculoDistanciaM()
        {
            distancia_m = RAIZ(Math.Pow(X , 2) + Math.Pow(Y ,2));
        }
        #endregion

        #region contra_azimute
        public double contra_azimute_decimal { get; set; }
        public double contra_azimute_grau { get; set; }
        public double contra_azimute_minuto { get; set; }
        public double contra_azimute_segundo { get; set; }

        public void CalculoContraAzimuteDecimal()
        {
            if (X < 0)
            {
                contra_azimute_decimal = 360 - (GRAUS(ACOS(Y / (RAIZ(Math.Pow(Y , 2) + Math.Pow(X , 2))))));
            }
            else
            {
                contra_azimute_decimal = (GRAUS(ACOS(Y / (RAIZ(Math.Pow(Y, 2) + Math.Pow(X, 2))))));
            }
        }

        public void CalculoContraAzimuteGrau()
        {
            contra_azimute_grau = (INT(contra_azimute_decimal * SINAL(contra_azimute_decimal)) * SINAL(contra_azimute_decimal));
        }

        public void CalculoContraAzimuteMinuto()
        {
            contra_azimute_minuto = INT((contra_azimute_decimal * SINAL(contra_azimute_decimal) - (INT(contra_azimute_decimal * SINAL(contra_azimute_decimal)))) * 60);
        }

        public void CalculoContraAzimuteSegundo()
        {
            contra_azimute_segundo = ((((contra_azimute_decimal * SINAL(contra_azimute_decimal) - (INT(contra_azimute_decimal * SINAL(contra_azimute_decimal)))) * 60)) 
                - INT((((contra_azimute_decimal * SINAL(contra_azimute_decimal) - (INT(contra_azimute_decimal * SINAL(contra_azimute_decimal)))) * 60)))) * 60;
        }

        #endregion


        #region AzimuteB
        public double azimute_b_decimal { get; set; }
        public double azimute_b_grau { get; set; }
        public double azimute_b_minuto { get; set; }
        public double azimute_b_segundo { get; set; }

        public void CalculoAzimuteBDecimal()
        {
            if (contra_azimute_decimal > 180)
            {
                azimute_b_decimal = contra_azimute_decimal - 180;
            }
            else
            {
                azimute_b_decimal = contra_azimute_decimal + 180;
            }
        }

        public void CalculoAzimuteBGrau()
        {
            azimute_b_grau = (INT(azimute_b_decimal * SINAL(azimute_b_decimal)) * SINAL(azimute_b_decimal));
        }

        public void CalculoAzimuteBMinuto()
        {
            azimute_b_minuto = INT((azimute_b_decimal * SINAL(azimute_b_decimal) - (INT(azimute_b_decimal * SINAL(azimute_b_decimal)))) * 60);
        }

        public void CalculoAzimuteBSegundo()
        {
            azimute_b_segundo = ((((azimute_b_decimal * SINAL(azimute_b_decimal) - (INT(azimute_b_decimal * SINAL(azimute_b_decimal)))) * 60)) 
                - INT((((azimute_b_decimal * SINAL(azimute_b_decimal) - (INT(azimute_b_decimal * SINAL(azimute_b_decimal)))) * 60)))) * 60;
        }

        #endregion

        #region rumo_gds
        public double rumo_gds { get; set; }
        public void CalculoRumoGDS()
        {
            if (azimute_b_decimal >=0 && azimute_b_decimal < 90)
            {
                rumo_gds = azimute_b_decimal;
            }
            else if (azimute_b_decimal >= 90 && azimute_b_decimal <= 180)
            {
                rumo_gds = 180 - azimute_b_decimal;
            }
            else if (azimute_b_decimal > 180 && azimute_b_decimal <= 270)
            {
                rumo_gds = azimute_b_decimal - 180;
            }
            else
            {
                rumo_gds = 360 - azimute_b_decimal;
            }
        }
        #endregion

        #region quadrante
        public string quadrante { get; set; }
        public void CalculoQuadrante()
        {
            if (azimute_b_decimal >= 0 && azimute_b_decimal < 90)
            {
                quadrante = "NE";
            }
            else if (azimute_b_decimal >= 90 && azimute_b_decimal <= 180)
            {
                quadrante = "SE";
            }
            else if (azimute_b_decimal > 180 && azimute_b_decimal <= 270)
            {
                quadrante = "SW";
            }
            else
            {
                quadrante = "NO";
            }
        }
        #endregion

        #region dif_hora
        public double dif_hora { get; set; }
        public void CalculoDiffHora()
        {
            dif_hora = data_b.Subtract(data_a).TotalHours;
        }
        #endregion

        #region velocidade_km
        public double velocidade_km { get; set; }
        public void CalculoVelocidadeKm()
        {
            velocidade_km = (distancia_m / 1000.0) / dif_hora;
        }
        #endregion

        #region velocidade_knots
        public double velocidade_knots { get; set; }
        public void CalculoVelocidadeKnots()
        {
            velocidade_knots = velocidade_km * 0.539957;
        }
        #endregion
    }
}
