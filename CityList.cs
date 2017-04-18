using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PENENTUAN_JALUR_TERPENDEK
{
    public class CityList
    {
        enum Cities
        {
            Kamizaun, Garuda_Spadem, PGT_Spadem, Mandala_Muli, Ahmad_Yani, RE_Martadinata,
            Parakomando, Prajurit, Brawijaya, Ermasu_Kuburan, Ermasu, Seringgu, Mandala,
            Mandala_Polres_Lama, Polder, Trikora
        };

        public List<String> names = new List<String>{
            "Jalan Kamizaun Mopah Lama",
            "Jalan Garuda Spadem",
            "Jalan PGT Spadem",
            "Jalan Raya Mandala Muli",
            "Jalan Ahmad Yani",
            "Jalan RE Martadinata",
            "Jalan Parakomando",
            "Jalan Prajurit",
            "Jalan Brawijaya",
            "Jalan Ermasu Kuburan",
            "Jalan Ermasu",
            "Jalan Seringgu",
            "Jalan Raya Mandala",
            "Jalan Raya Mandala Polres Lama",
            "Jalan Polder",
            //"Jalan TMP Trikora",
            "Jalan Trikora"
        };

        private double[,] actual = new double[16, 16];
        private double[,] heuristic;

        private void SetActual()
        {
            actual[(int)Cities.Kamizaun, (int)Cities.Garuda_Spadem] = 1.2;
            actual[(int)Cities.Garuda_Spadem, (int)Cities.PGT_Spadem] = 0.2;
            actual[(int)Cities.PGT_Spadem, (int)Cities.Mandala_Muli] = 0.2;
            actual[(int)Cities.PGT_Spadem, (int)Cities.Ahmad_Yani] = 0.4;
            actual[(int)Cities.PGT_Spadem, (int)Cities.RE_Martadinata] = 0.5;
            actual[(int)Cities.PGT_Spadem, (int)Cities.Garuda_Spadem] = 0.2;
            actual[(int)Cities.Mandala_Muli, (int)Cities.Ahmad_Yani] = 0.3;
            actual[(int)Cities.Mandala_Muli, (int)Cities.Mandala] = 3.5;
            actual[(int)Cities.Mandala_Muli, (int)Cities.RE_Martadinata] = 0.3;
            //actual[(int)Cities.Mandala_Muli, (int)Cities.PGT_Spadem] = 0.1;
            actual[(int)Cities.Ahmad_Yani, (int)Cities.RE_Martadinata] = 0.1;
            actual[(int)Cities.Ahmad_Yani, (int)Cities.Parakomando] = 3.3;
            actual[(int)Cities.Ahmad_Yani, (int)Cities.Prajurit] = 3.4;
            actual[(int)Cities.Ahmad_Yani, (int)Cities.Brawijaya] = 3.5;
            //actual[(int)Cities.Ahmad_Yani, (int)Cities.Mandala] = 3.2;
            //actual[(int)Cities.Ahmad_Yani, (int)Cities.Mandala_Muli] = 0.3;
            actual[(int)Cities.RE_Martadinata, (int)Cities.Ahmad_Yani] = 0.1;
            actual[(int)Cities.RE_Martadinata, (int)Cities.Parakomando] = 3.2;
            actual[(int)Cities.RE_Martadinata, (int)Cities.Prajurit] = 3.3;
            actual[(int)Cities.RE_Martadinata, (int)Cities.Brawijaya] = 3.4;
            actual[(int)Cities.Parakomando, (int)Cities.RE_Martadinata] = 3.2;
            actual[(int)Cities.Parakomando, (int)Cities.Prajurit] = 0.1;
            actual[(int)Cities.Parakomando, (int)Cities.Ermasu] = 0.1;
            //actual[(int)Cities.Parakomando, (int)Cities.Brawijaya] = 0.2;
            actual[(int)Cities.Parakomando, (int)Cities.Seringgu] = 0.2;
            actual[(int)Cities.Parakomando, (int)Cities.Mandala] = 0.5;
            actual[(int)Cities.Prajurit, (int)Cities.Ahmad_Yani] = 3.4;
            actual[(int)Cities.Prajurit, (int)Cities.Parakomando] = 0.1;
            actual[(int)Cities.Prajurit, (int)Cities.RE_Martadinata] = 3.3;
            actual[(int)Cities.Prajurit, (int)Cities.Brawijaya] = 0.1;
            actual[(int)Cities.Prajurit, (int)Cities.Ermasu_Kuburan] = 0.5;
            actual[(int)Cities.Prajurit, (int)Cities.Ermasu] = 0.3;
            //actual[(int)Cities.Prajurit, (int)Cities.TMP_Trikora] = 0.7;
            actual[(int)Cities.Brawijaya, (int)Cities.Ahmad_Yani] = 3.5;
            actual[(int)Cities.Brawijaya, (int)Cities.Prajurit] = 0.1;
            actual[(int)Cities.Brawijaya, (int)Cities.Parakomando] = 0.1;
            actual[(int)Cities.Brawijaya, (int)Cities.Polder] = 3.1;
            //actual[(int)Cities.Brawijaya, (int)Cities.TMP_Trikora] = 0.7;
            actual[(int)Cities.Brawijaya, (int)Cities.Trikora] = 2.1;
            actual[(int)Cities.Ermasu_Kuburan, (int)Cities.Prajurit] = 0.5;
            actual[(int)Cities.Ermasu_Kuburan, (int)Cities.Ermasu] = 0.3;
            actual[(int)Cities.Ermasu_Kuburan, (int)Cities.Mandala] = 0.5;
            actual[(int)Cities.Ermasu_Kuburan, (int)Cities.Mandala_Polres_Lama] = 0.3;
            //actual[(int)Cities.Ermasu_Kuburan, (int)Cities.TMP_Trikora] = 0.4;
            actual[(int)Cities.Ermasu, (int)Cities.RE_Martadinata] = 3.6;
            actual[(int)Cities.Ermasu, (int)Cities.Ahmad_Yani] = 3.7;
            actual[(int)Cities.Ermasu, (int)Cities.Prajurit] = 0.3;
            actual[(int)Cities.Ermasu, (int)Cities.Parakomando] = 0.1;
            actual[(int)Cities.Ermasu, (int)Cities.Ermasu_Kuburan] = 0.3;
            actual[(int)Cities.Ermasu, (int)Cities.Mandala_Polres_Lama] = 0.7;
            //actual[(int)Cities.Ermasu, (int)Cities.TMP_Trikora] = 0.3;
            actual[(int)Cities.Seringgu, (int)Cities.Parakomando] = 0.2;
            actual[(int)Cities.Seringgu, (int)Cities.Mandala] = 0.1;
            actual[(int)Cities.Seringgu, (int)Cities.Mandala_Polres_Lama] = 1.1;
            actual[(int)Cities.Mandala, (int)Cities.Parakomando] = 0.5;
            actual[(int)Cities.Mandala, (int)Cities.Seringgu] = 0.1;
            actual[(int)Cities.Mandala, (int)Cities.Mandala_Polres_Lama] = 1.2;
            actual[(int)Cities.Mandala, (int)Cities.Mandala_Muli] = 3.5;
            actual[(int)Cities.Mandala_Polres_Lama, (int)Cities.Ermasu] = 0.7;
            actual[(int)Cities.Mandala_Polres_Lama, (int)Cities.Ermasu_Kuburan] = 0.3;
            actual[(int)Cities.Mandala_Polres_Lama, (int)Cities.Mandala] = 1.2;
            actual[(int)Cities.Mandala_Polres_Lama, (int)Cities.Seringgu] = 1.1;
            actual[(int)Cities.Mandala_Polres_Lama, (int)Cities.Trikora] = 0.9;
            actual[(int)Cities.Polder, (int)Cities.Parakomando] = 3;
            actual[(int)Cities.Polder, (int)Cities.Ermasu_Kuburan] = 2.5;
            actual[(int)Cities.Polder, (int)Cities.Mandala_Polres_Lama] = 0.7;
            //actual[(int)Cities.Polder, (int)Cities.TMP_Trikora] = 1.2;
            actual[(int)Cities.Polder, (int)Cities.Trikora] = 0.7;
            actual[(int)Cities.Trikora, (int)Cities.Mandala_Polres_Lama] = 0.9;
            //actual[(int)Cities.TMP_Trikora, (int)Cities.Ermasu] = 0.3;
            //actual[(int)Cities.TMP_Trikora, (int)Cities.Ermasu_Kuburan] = 0.4;
            //actual[(int)Cities.TMP_Trikora, (int)Cities.Polder] = 1.2;
            //actual[(int)Cities.TMP_Trikora, (int)Cities.Trikora] = 1.9;
        }

        private void SetHeuristic()
        {
            heuristic = new double[,]{
                {	0	,	271	,	116	,	166	,	116	,	186	,	23	,	200	,	113	,	144	,	47	,	47	,	163	,	186	,	209	,	130	}	,
                {	271	,	0	,	253	,	299	,	364	,	84	,	271	,	90	,	294	,	316	,	47	,	311	,	154	,	390	,	416	,	241	}	,
                {	116	,	253	,	0	,	61	,	130	,	164	,	99	,	164	,	60	,	66	,	47	,	104	,	101	,	140	,	164	,	21	}	,
                {	166	,	299	,	61	,	0	,	131	,	226	,	143	,	116	,	79	,	43	,	47	,	139	,	273	,	107	,	126	,	64	}	,
                {	116	,	364	,	130	,	131	,	0	,	273	,	101	,	280	,	69	,	93	,	47	,	71	,	224	,	83	,	109	,	151	}	,
                {	186	,	84	,	164	,	226	,	273	,	0	,	187	,	34	,	219	,	241	,	47	,	216	,	84	,	313	,	337	,	169	}	,
                {	23	,	271	,	99	,	143	,	101	,	187	,	0	,	200	,	70	,	121	,	47	,	31	,	153	,	164	,	186	,	110	}	,
                {	200	,	90	,	164	,	116	,	280	,	34	,	200	,	0	,	214	,	231	,	47	,	216	,	64	,	303	,	326	,	147	}	,
                {	113	,	294	,	60	,	79	,	69	,	219	,	70	,	214	,	0	,	47	,	47	,	63	,	156	,	99	,	123	,	80	}	,
                {	144	,	316	,	66	,	43	,	93	,	241	,	121	,	231	,	47	,	0	,	47	,	109	,	166	,	74	,	97	,	83	}	,
                {	1	,	316	,	66	,	43	,	93	,	241	,	121	,	231	,	47	,	2	,	0	,	2	,	166	,	74	,	97	,	83	}	,
                {	47	,	311	,	104	,	139	,	71	,	216	,	31	,	216	,	63	,	109	,	47	,	0	,	169	,	141	,	164	,	121	}	,
                {	163	,	154	,	101	,	273	,	224	,	84	,	153	,	64	,	156	,	166	,	47	,	169	,	0	,	240	,	264	,	83	}	,
                {	186	,	390	,	140	,	107	,	83	,	313	,	164	,	303	,	99	,	74	,	47	,	141	,	240	,	0	,	24	,	157	}	,
                {	209	,	416	,	164	,	126	,	109	,	337	,	186	,	326	,	123	,	97	,	47	,	164	,	264	,	24	,	0	,	180	}	,
                {	130	,	241	,	21	,	64	,	151	,	169	,	110	,	147	,	80	,	83	,	47	,	121	,	83	,	157	,	180	,	0	}	,
            };
        }

        public CityList()
        {
            SetActual();
            SetHeuristic();
        }

        public int GetCityIndex(String cityname)
        {
            return names.IndexOf(cityname);
        }

        public List<int> GetCityNeighbor(int index)
        {
            List<int> neighbour = new List<int>();
            for (int i = 0; i < 16; i++)
                if (actual[index, i] > 0)
                {
                    neighbour.Add(i);
                }
            return neighbour;
        }

        public double GetActualCost(int index1, int index2)
        {
            return actual[index1, index2];
        }

        public double GetHeuristic(int index1, int index2)
        {
            return heuristic[index1, index2];
        }


        internal void UpdateActualCost(int index1, int index2, int value)
        {
            actual[index1, index2] = value;
        }
    }
}
