using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Web.Script;
using System.Web.Script.Serialization;
using Microsoft.VisualBasic;

namespace PENENTUAN_JALUR_TERPENDEK
{
    public partial class FORM_PENENTUAN_JALUR : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FORM_PENENTUAN_JALUR()
        {
            InitializeComponent();
        }

        private CityList listCity = new CityList();
        private DataCombo modkonek = new DataCombo();
        private List<int> neighbour;
        private List<int> openList;
        private List<int> closeList;
        private bool[] flag;
        private int root;

        private void FORM_PENENTUAN_JALUR_Load(object sender, EventArgs e)
        {
            
        }

        private string NOMOR_OTOMATIS()
        {
            using (SqlConnection connect = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DB_Sistem_Penentuan_Jalur_Terpendek_Kota_Merauke_A_Star_Greedy;Integrated Security=True"))
            {
                string str = "SELECT (ABS(CHECKSUM(NEWID())) % 100001) + ((ABS(CHECKSUM(NEWID())) % 100001) * 0.00001) [KODE_JALAN_AWAL]";
                using (SqlCommand cmd = new SqlCommand(str, connect))
                {

                    var _with1 = cmd;
                    _with1.CommandType = CommandType.Text;
                    _with1.CommandText = str;
                    _with1.Connection = connect;
                    _with1.Connection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            return "J-" + dr.GetValue(0);
                        }
                        return string.Empty;
                    }
                }
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
             //webBrowser1.DocumentText = "<!DOCTYPE html>" + Constants.vbCr + Constants.vbLf +
             //  "  <html> " + Constants.vbCr + Constants.vbLf + "  <form> " + Constants.vbCr + Constants.vbLf +
             //  "  <head> " + Constants.vbCr + Constants.vbLf + "  <meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" /> " +
             //  Constants.vbCr + Constants.vbLf + "  <title>Google Maps Multiple Markers</title> " + Constants.vbCr + Constants.vbLf +
             //  "  <script src=\"http://maps.google.com/maps/api/js?sensor=false\" " + Constants.vbCr + Constants.vbLf +
             //  "  type=\"text/javascript\"></script>" + Constants.vbCr + Constants.vbLf + "</head> " + Constants.vbCr + Constants.vbLf +
             //  "  <body>" + Constants.vbCr + Constants.vbLf + "  <div id=\"map\" style=\"width: 1000px; height: 400px;\"></div>" +
             //  Constants.vbCr + Constants.vbLf + Constants.vbCr + Constants.vbLf + "  <script type=\"text/javascript\">" +
             //  Constants.vbCr + Constants.vbLf + "    var locations = [" + Constants.vbCr + Constants.vbLf +
             //  "  ['" + this.comboBoxEx1.SelectedItem + "', " + this.TXTKOORDINATASAL.Text + ", 1]," + Constants.vbCr + Constants.vbLf +
             //  "  ['" + this.comboBoxEx2.SelectedItem + "', " + this.TXTKOORDINATTUJUAN.Text + ", 2]" + Constants.vbCr + Constants.vbLf + "    ];" + Constants.vbCr + Constants.vbLf + Constants.vbCr + Constants.vbLf + "    var map = new google.maps.Map(document.getElementById('map'), {" + Constants.vbCr + Constants.vbLf + "      zoom: 15," + Constants.vbCr + Constants.vbLf +
             //  "  center: new google.maps.LatLng(-8.500401, 140.407156)," + Constants.vbCr + Constants.vbLf + 
             //  "  mapTypeId: google.maps.MapTypeId.ROADMAP" + Constants.vbCr + Constants.vbLf + "    });" + Constants.vbCr + Constants.vbLf + Constants.vbCr + Constants.vbLf + 
             //  "  var infowindow = new google.maps.InfoWindow();" + Constants.vbCr + Constants.vbLf + Constants.vbCr + Constants.vbLf +
             //  "  var marker, i;" + Constants.vbCr + Constants.vbLf + Constants.vbCr + Constants.vbLf +
             //  "  for (i = 0; i < locations.length; i++) {  " + Constants.vbCr + Constants.vbLf +
             //  "  marker = new google.maps.Marker({" + Constants.vbCr + Constants.vbLf +
             //  "  position: new google.maps.LatLng(locations[i][1], locations[i][2])," + Constants.vbCr + Constants.vbLf +
             //  "  map: map" + Constants.vbCr + Constants.vbLf + "      });" + Constants.vbCr + Constants.vbLf + Constants.vbCr + Constants.vbLf +
             //  "  google.maps.event.addListener(marker, 'click', (function(marker, i) {" + Constants.vbCr + Constants.vbLf +
             //  "  return function() {" + Constants.vbCr + Constants.vbLf + "          infowindow.setContent(locations[i][0]);" + Constants.vbCr + Constants.vbLf + "          infowindow.open(map, marker);" + Constants.vbCr + Constants.vbLf + "        }" + Constants.vbCr + Constants.vbLf + "      })(marker, i));" + Constants.vbCr + Constants.vbLf + "    }" + Constants.vbCr + Constants.vbLf +
             //  "  </script>" + Constants.vbCr + Constants.vbLf + "</form>" + Constants.vbCr + Constants.vbLf + "</body>" + Constants.vbCr + Constants.vbLf + "</html>"
             //  + Constants.vbCr + Constants.vbLf + Constants.vbCr + Constants.vbLf;

             webBrowser1.DocumentText = "<!DOCTYPE html>" + Constants.vbCr + Constants.vbLf +
               "  <html> " + Constants.vbCr + Constants.vbLf + 
               "  <head> " + Constants.vbCr + Constants.vbLf +
               "  <meta name=\"viewport\" content=\"initial-scale=1.0, user-scalable=no\" /> " + Constants.vbCr + Constants.vbLf + 
               "  <title>Google Maps Multiple Markers</title> " + Constants.vbCr + Constants.vbLf +
               "  <script src=\"http://maps.googleapis.com/maps/api/js?sensor=false\" " + Constants.vbCr + Constants.vbLf +
               "  type=\"text/javascript\"></script>" + Constants.vbCr + Constants.vbLf + 
               "  <script type=\"text/javascript\">" + Constants.vbCr + Constants.vbLf +
               "  var markers = [" + Constants.vbCr + Constants.vbLf +
               "  { " + Constants.vbCr + Constants.vbLf +
               "  \"Title\": " + "'" + this.comboBoxEx1.SelectedItem + "'" + ", " + Constants.vbCr + Constants.vbLf +
               "  \"lat\": " + "'" + this.TXTKOORDINATASAL_LAT.Text + "'" + ", " + Constants.vbCr + Constants.vbLf +
               "  \"lng\": " + "'" + this.TXTKOORDINATASAL_LONG.Text + "'" + ", " + Constants.vbCr + Constants.vbLf +
               "  \"description\" : " + "'" + this.comboBoxEx1.SelectedItem + "'" + Constants.vbCr + Constants.vbLf +
               "  } " + Constants.vbCr + Constants.vbLf +
               "  , " + Constants.vbCr + Constants.vbLf +
               "  {" + Constants.vbCr + Constants.vbLf +
               "  \"Title\": " + "'" + this.comboBoxEx2.SelectedItem + "'" + ", " + Constants.vbCr + Constants.vbLf +
               "  \"lat\": " + "'" + this.TXTKOORDINATTUJUAN_LAT.Text + "'" + ", " + Constants.vbCr + Constants.vbLf +
               "  \"lng\": " + "'" + this.TXTKOORDINATTUJUAN_LONG.Text + "'" + ", " + Constants.vbCr + Constants.vbLf +
               "  \"description\": " + "'" + this.comboBoxEx2.SelectedItem + "'" + Constants.vbCr + Constants.vbLf +
               "  } " + Constants.vbCr + Constants.vbLf +
               "  ]; " + Constants.vbCr + Constants.vbLf +
               "  </script>" + Constants.vbCr + Constants.vbLf +
               "  <script type=\"text/javascript\">" + Constants.vbCr + Constants.vbLf +
               "  window.onload = function () {" + Constants.vbCr + Constants.vbLf +
               "  var mapOptions = {" + Constants.vbCr + Constants.vbLf +
               "  center: new google.maps.LatLng(markers[0].lat, markers[0].lng)," + Constants.vbCr + Constants.vbLf +
               "  zoom: 16," + Constants.vbCr + Constants.vbLf +
               "  mapTypeId: google.maps.MapTypeId.ROADMAP" + Constants.vbCr + Constants.vbLf +
               "  };" + Constants.vbCr + Constants.vbLf +
               "  var path = new google.maps.MVCArray();" + Constants.vbCr + Constants.vbLf +
               "  var service = new google.maps.DirectionsService();" + Constants.vbCr + Constants.vbLf +
               "  var infoWindow = new google.maps.InfoWindow();" + Constants.vbCr + Constants.vbLf +
               "  var map = new google.maps.Map(document.getElementById(\"dvMap\"), mapOptions);" + Constants.vbCr + Constants.vbLf +
               "  var poly = new google.maps.Polyline({ map: map, strokeColor: '#FF8200' });" + Constants.vbCr + Constants.vbLf +
               "  var lat_lng = new Array();" + Constants.vbCr + Constants.vbLf +
               "  for (i = 0; i < markers.length; i++) {" + Constants.vbCr + Constants.vbLf +
               "  var data = markers[i]" + Constants.vbCr + Constants.vbLf +
               "  var myLatlng = new google.maps.LatLng(data.lat, data.lng);" + Constants.vbCr + Constants.vbLf +
               "  lat_lng.push(myLatlng);" + Constants.vbCr + Constants.vbLf +
               "  var marker = new google.maps.Marker({" + Constants.vbCr + Constants.vbLf +
               "  position: myLatlng," + Constants.vbCr + Constants.vbLf +
               "  map: map," + Constants.vbCr + Constants.vbLf +
               "  title: data.title" + Constants.vbCr + Constants.vbLf +
               "  });" + Constants.vbCr + Constants.vbLf +
               "  (function (marker, data) {" + Constants.vbCr + Constants.vbLf +
               "  google.maps.event.addListener(marker, \"click\", function (e) {" + Constants.vbCr + Constants.vbLf +
               "  infoWindow.setContent(data.description);" + Constants.vbCr + Constants.vbLf +
               "  infoWindow.open(map, marker);" + Constants.vbCr + Constants.vbLf +
               "  });" + Constants.vbCr + Constants.vbLf +
               "  })(marker, data);" + Constants.vbCr + Constants.vbLf +
               "  }" + Constants.vbCr + Constants.vbLf +
               "  for (var i = 0; i < lat_lng.length; i++) {" + Constants.vbCr + Constants.vbLf +
               "  if ((i + 1) < lat_lng.length) {" + Constants.vbCr + Constants.vbLf +
               "  var src = lat_lng[i];" + Constants.vbCr + Constants.vbLf +
               "  var des = lat_lng[i + 1];" + Constants.vbCr + Constants.vbLf +
               "  path.push(src);" + Constants.vbCr + Constants.vbLf +
               "  poly.setPath(path);" + Constants.vbCr + Constants.vbLf +
               "  service.route({" + Constants.vbCr + Constants.vbLf +
               "  origin: src," + Constants.vbCr + Constants.vbLf +
               "  destination: des," + Constants.vbCr + Constants.vbLf +
               "  travelMode: google.maps.DirectionsTravelMode.DRIVING" + Constants.vbCr + Constants.vbLf +
               "  }, function (result, status) {" + Constants.vbCr + Constants.vbLf +
               "  if (status == google.maps.DirectionsStatus.OK) {" + Constants.vbCr + Constants.vbLf +
               "  for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {" + Constants.vbCr + Constants.vbLf +
               "  path.push(result.routes[0].overview_path[i]);" + Constants.vbCr + Constants.vbLf +
               "  }" + Constants.vbCr + Constants.vbLf +
               "  }" + Constants.vbCr + Constants.vbLf +
               "  });" + Constants.vbCr + Constants.vbLf +
               "  }" + Constants.vbCr + Constants.vbLf +
               "  }" + Constants.vbCr + Constants.vbLf +
               "  }" + Constants.vbCr + Constants.vbLf +
               "  </script>" + Constants.vbCr + Constants.vbLf +
               "  <div id=\"dvMap\" style=\"width: 1000px; height: 1000px\"></div>" + Constants.vbCr + Constants.vbLf +
               "  </head>" + Constants.vbCr + Constants.vbLf + 
               "  </html>" + Constants.vbCr + Constants.vbLf;
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.SelectedIndex == 0)
            {
                this.TXTKOORDINATASAL_LAT.Text = "-8.531020";
                this.TXTKOORDINATASAL_LONG.Text = "140.418570";
            }
            else
                if (this.comboBoxEx1.SelectedIndex == 1)
                {
                    this.TXTKOORDINATASAL_LAT.Text = "-8.521698";
                    this.TXTKOORDINATASAL_LONG.Text = "140.412313";
                }
                else
                    if (this.comboBoxEx1.SelectedIndex == 2)
                    {
                        this.TXTKOORDINATASAL_LAT.Text = "-8.520896";
                        this.TXTKOORDINATASAL_LONG.Text = "140.411422";
                    }
                    else
                        if (this.comboBoxEx1.SelectedIndex == 3)
                        {
                            this.TXTKOORDINATASAL_LAT.Text = "-8.516920";
                            this.TXTKOORDINATASAL_LONG.Text = "140.407876";
                        }
                        else
                            if (this.comboBoxEx1.SelectedIndex == 4)
                            {
                                this.TXTKOORDINATASAL_LAT.Text = "-8.511237";
                                this.TXTKOORDINATASAL_LONG.Text = "140.406353";
                            }
                            else
                                if (this.comboBoxEx1.SelectedIndex == 5)
                                {
                                    this.TXTKOORDINATASAL_LAT.Text = "-8.509447";
                                    this.TXTKOORDINATASAL_LONG.Text = "140.408527";
                                }
                                else
                                    if (this.comboBoxEx1.SelectedIndex == 6)
                                    {
                                        this.TXTKOORDINATASAL_LAT.Text = "-8.496786";
                                        this.TXTKOORDINATASAL_LONG.Text = "140.397379";
                                    }
                                    else
                                        if (this.comboBoxEx1.SelectedIndex == 7)
                                        {
                                            this.TXTKOORDINATASAL_LAT.Text = "-8.494646";
                                            this.TXTKOORDINATASAL_LONG.Text = "140.400352";
                                        }
                                        else
                                            if (this.comboBoxEx1.SelectedIndex == 8)
                                            {
                                                this.TXTKOORDINATASAL_LAT.Text = "-8.494479";
                                                this.TXTKOORDINATASAL_LONG.Text = "140.401946";
                                            }
                                            else
                                                if (this.comboBoxEx1.SelectedIndex == 9)
                                                {
                                                    this.TXTKOORDINATASAL_LAT.Text = "-8.486854";
                                                    this.TXTKOORDINATASAL_LONG.Text = "140.393814";
                                                }
                                                else
                                                    if (this.comboBoxEx1.SelectedIndex == 10)
                                                    {
                                                        this.TXTKOORDINATASAL_LAT.Text = "-8.494694";
                                                        this.TXTKOORDINATASAL_LONG.Text = "140.397391";
                                                    }
                                                    else
                                                        if (this.comboBoxEx1.SelectedIndex == 11)
                                                        {
                                                            this.TXTKOORDINATASAL_LAT.Text = "-8.497949";
                                                            this.TXTKOORDINATASAL_LONG.Text = "140.394252";
                                                        }
                                                        else
                                                            if (this.comboBoxEx1.SelectedIndex == 12)
                                                            {
                                                                this.TXTKOORDINATASAL_LAT.Text = "-8.497799";
                                                                this.TXTKOORDINATASAL_LONG.Text = "140.395534";
                                                            }
                                                            else
                                                                if (this.comboBoxEx1.SelectedIndex == 13)
                                                                {
                                                                    this.TXTKOORDINATASAL_LAT.Text = "-8.483205";
                                                                    this.TXTKOORDINATASAL_LONG.Text = "140.388990";
                                                                }
                                                                else
                                                                    if (this.comboBoxEx1.SelectedIndex == 14)
                                                                    {
                                                                        this.TXTKOORDINATASAL_LAT.Text = "-8.480902";
                                                                        this.TXTKOORDINATASAL_LONG.Text = "140.396067";
                                                                    }
                                                                    else
                                                                        if (this.comboBoxEx1.SelectedIndex == 15)
                                                                        {
                                                                            this.TXTKOORDINATASAL_LAT.Text = "-8.488881";
                                                                            this.TXTKOORDINATASAL_LONG.Text = "140.399730";
                                                                        }
                                                                        else
                                                                            if (this.comboBoxEx1.SelectedIndex == 16)
                                                                            {
                                                                                this.TXTKOORDINATASAL_LAT.Text = "-8.487869";
                                                                                this.TXTKOORDINATASAL_LONG.Text = "140.398796";
                                                                            }
           
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx2.SelectedIndex == 0)
            {
                this.TXTKOORDINATTUJUAN_LAT.Text = "-8.531020";
                this.TXTKOORDINATTUJUAN_LONG.Text = "140.418570";
            }
            else
                if (this.comboBoxEx2.SelectedIndex == 1)
                {
                    this.TXTKOORDINATTUJUAN_LAT.Text = "-8.521698";
                    this.TXTKOORDINATTUJUAN_LONG.Text = "140.412313";
                }
                else
                    if (this.comboBoxEx2.SelectedIndex == 2)
                    {
                        this.TXTKOORDINATTUJUAN_LAT.Text = "-8.520896";
                        this.TXTKOORDINATTUJUAN_LONG.Text = "140.411422";
                    }
                    else
                        if (this.comboBoxEx2.SelectedIndex == 3)
                        {
                            this.TXTKOORDINATTUJUAN_LAT.Text = "-8.516920";
                            this.TXTKOORDINATTUJUAN_LONG.Text = "140.407876";
                        }
                        else
                            if (this.comboBoxEx2.SelectedIndex == 4)
                            {
                                this.TXTKOORDINATTUJUAN_LAT.Text = "-8.511237";
                                this.TXTKOORDINATTUJUAN_LONG.Text = "140.406353";
                            }
                            else
                                if (this.comboBoxEx2.SelectedIndex == 5)
                                {
                                    this.TXTKOORDINATTUJUAN_LAT.Text = "-8.509447";
                                    this.TXTKOORDINATTUJUAN_LONG.Text = "140.408527";
                                }
                                else
                                    if (this.comboBoxEx2.SelectedIndex == 6)
                                    {
                                        this.TXTKOORDINATTUJUAN_LAT.Text = "-8.496786";
                                        this.TXTKOORDINATTUJUAN_LONG.Text = "140.397379";
                                    }
                                    else
                                        if (this.comboBoxEx2.SelectedIndex == 7)
                                        {
                                            this.TXTKOORDINATTUJUAN_LAT.Text = "-8.494646";
                                            this.TXTKOORDINATTUJUAN_LONG.Text = "140.400352";
                                        }
                                        else
                                            if (this.comboBoxEx2.SelectedIndex == 8)
                                            {
                                                this.TXTKOORDINATTUJUAN_LAT.Text = "-8.494479";
                                                this.TXTKOORDINATTUJUAN_LONG.Text = "140.401946";
                                            }
                                            else
                                                if (this.comboBoxEx2.SelectedIndex == 9)
                                                {
                                                    this.TXTKOORDINATTUJUAN_LAT.Text = "-8.486854";
                                                    this.TXTKOORDINATTUJUAN_LONG.Text = "140.393814";
                                                }
                                                else
                                                    if (this.comboBoxEx2.SelectedIndex == 10)
                                                    {
                                                        this.TXTKOORDINATTUJUAN_LAT.Text = "-8.494694";
                                                        this.TXTKOORDINATTUJUAN_LONG.Text = "140.397391";
                                                    }
                                                    else
                                                        if (this.comboBoxEx2.SelectedIndex == 11)
                                                        {
                                                            this.TXTKOORDINATTUJUAN_LAT.Text = "-8.497949";
                                                            this.TXTKOORDINATTUJUAN_LONG.Text = "140.394252";
                                                        }
                                                        else
                                                            if (this.comboBoxEx2.SelectedIndex == 12)
                                                            {
                                                                this.TXTKOORDINATTUJUAN_LAT.Text = "-8.497799";
                                                                this.TXTKOORDINATTUJUAN_LONG.Text = "140.395534";
                                                            }
                                                            else
                                                                if (this.comboBoxEx2.SelectedIndex == 13)
                                                                {
                                                                    this.TXTKOORDINATTUJUAN_LAT.Text = "-8.483205";
                                                                    this.TXTKOORDINATTUJUAN_LONG.Text = "140.388990";
                                                                }
                                                                else
                                                                    if (this.comboBoxEx2.SelectedIndex == 14)
                                                                    {
                                                                        this.TXTKOORDINATTUJUAN_LAT.Text = "-8.480902";
                                                                        this.TXTKOORDINATTUJUAN_LONG.Text = "140.396067";
                                                                    }
                                                                    else
                                                                        if (this.comboBoxEx2.SelectedIndex == 15)
                                                                        {
                                                                            this.TXTKOORDINATTUJUAN_LAT.Text = "-8.488881";
                                                                            this.TXTKOORDINATTUJUAN_LONG.Text = "140.399730";
                                                                        }
                                                                        else
                                                                            if (this.comboBoxEx2.SelectedIndex == 16)
                                                                            {
                                                                                this.TXTKOORDINATTUJUAN_LAT.Text = "-8.487869";
                                                                                this.TXTKOORDINATTUJUAN_LONG.Text = "140.398796";
                                                                            }
        }

        public List<int> GreedySearch(int source, int destination)
        {
            closeList = new List<int>();
            flag = new bool[16];
            int current;
            bool state;
            state = true;
            root = source;
            closeList.Add(root);
            flag[root] = false;

            for (int i = 0; i < 16; i++)
            {
                flag[i] = true;
            }

            while (state == true)
            {
                current = closeList.Last();
                if (current == destination)
                {
                    state = false;
                    return closeList;
                }
                else
                {
                    neighbour = listCity.GetCityNeighbor(current);
                    int counter = 0, temp = 0, cek = 0, min = 999999;
                    while (neighbour.Count > counter)
                    {
                        if (flag[neighbour[counter]])
                        {
                            temp = Convert.ToInt32(listCity.GetHeuristic(destination, neighbour[counter]));
                            if (temp < min)
                            {
                                min = temp;
                                cek = counter;
                            }
                            flag[neighbour[counter]] = false;
                        }
                        counter++;
                    }
                    //try
                    //{
                    closeList.Add(neighbour[cek]);
                    //}
                    //catch (Exception salah)
                    //{
                    //    MessageBox.Show("PERHATIAN..  " + salah.Message, "Kesalahan");
                    //}

                    
                }
            }
            return closeList;
        }

        public List<int> ASearch(int source, int destination)
        {
            //closeList = new List<int>();
            //flag = new bool[16];
            //int current;
            //bool state;
            //state = true;
            //root = source;
            //closeList.Add(root);
            //flag[root] = false;

            //for (int i = 0; i < 16; i++)
            //{
            //    flag[i] = true;
            //}

            //while (state == true)
            //{
            //    current = closeList.Last();
            //    if (current == destination)
            //    {
            //        state = false;
            //        return closeList;
            //    }
            //    else
            //    {
            //        neighbour = listCity.GetCityNeighbor(current);
            //        int counter = 0, temp = 0, cek = 0, min = 999999;
            //        while (neighbour.Count > counter)
            //        {
            //            if (flag[neighbour[counter]])
            //            {
            //                temp = Convert.ToInt32(listCity.GetHeuristic(destination, neighbour[counter]));
            //                if (temp < min)
            //                {
            //                    min = temp;
            //                    cek = counter;
            //                }
            //                flag[neighbour[counter]] = false;
            //            }
            //            counter++;
            //        }
            //        closeList.Add(neighbour[cek]);
            //    }
            //}
            //return closeList;

            closeList = new List<int>();
            openList = new List<int>();
            flag = new bool[16];
            int current;
            bool state;
            for (int i = 0; i < 16; i++)
            {
                flag[i] = true;
            }

            state = true;
            root = source;
            closeList.Add(root);
            flag[root] = false;

            while (state == true)
            {
                current = closeList.Last();
                if (current == destination)
                {
                    state = false;
                    return closeList;
                }
                else
                {
                    neighbour = listCity.GetCityNeighbor(current);
                    int counter = 0, temp = 0, cek = 0, min = 999999, fact = 0;
                    while (counter < neighbour.Count)
                    {
                        openList.Add(neighbour[counter]);
                        counter++;
                    }
                    counter = 0;
                    while (openList.Count > counter)
                    {
                        temp = Convert.ToInt32(listCity.GetHeuristic(destination, openList[counter]));
                        fact = Convert.ToInt32(listCity.GetActualCost(current, openList[counter]));
                        temp += fact;
                        if ((temp) < min)
                        {
                            min = temp;
                            cek = counter;
                        }
                        //flag[neighbour[counter]] = false;
                        counter++;
                    }
                    closeList.Add(openList[cek]);
                    openList.RemoveAt(cek);
                }
            }
            return closeList;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Stack<int> path = new Stack<int>();
            Stack<int> path2 = new Stack<int>();

            if (comboMethod.SelectedIndex == 1)
            {
                closeList = ASearch(this.comboBoxEx1.SelectedIndex, this.comboBoxEx2.SelectedIndex);
            }
            else if (comboMethod.SelectedIndex == 2)
            {
                closeList = GreedySearch(comboBoxEx1.SelectedIndex, comboBoxEx2.SelectedIndex);
            }
            int temp = 0;
            txtJalur.Text = "";
            string pathName;
            double distance = 0.0;

            if (comboMethod.SelectedIndex != 0 && (comboMethod.SelectedIndex != 1 && comboMethod.SelectedIndex != 2))
            {
                while (path.Count != 0)
                {
                    path2.Push(path.Pop());
                }
            }
            else if (comboMethod.SelectedIndex == 1)
            {
                for (int i = 0; i < closeList.Count(); i++)
                {
                    temp = closeList[i];
                    pathName = listCity.names[temp];
                    txtJalur.Text = txtJalur.Text + " --> " + pathName;
                    this.richTextBox1.Text = pathName;
                    if (i + 1 != closeList.Count)
                        distance += listCity.GetActualCost(temp, closeList[i + 1]);
                }
            }
            else if (comboMethod.SelectedIndex == 2)
            {
                for (int i = 0; i < closeList.Count(); i++)
                {
                    temp = closeList[i];
                    pathName = listCity.names[temp];
                    txtJalur.Text = txtJalur.Text + " --> " + pathName;
                    this.richTextBox1.Text = pathName;
                    if (i + 1 != closeList.Count)
                        distance += listCity.GetActualCost(temp, closeList[i + 1]);
                }
            }
            else
                path2 = path;

            while (path2.Count != 0 && comboMethod.SelectedIndex != 1)
            {
                temp = path2.Peek();
                pathName = listCity.names[path2.Pop()];
                txtJalur.Text = txtJalur.Text + " --> " + pathName;
                this.richTextBox1.Text = pathName;
                if (path2.Count != 0)
                    distance += listCity.GetActualCost(temp, path2.Peek());
            }
            txtJalur.Text = txtJalur.Text + "\n Total Jarak: " + distance + "KM";
            this.richTextBox1.Text = this.richTextBox1.Text;
        }

    }
}