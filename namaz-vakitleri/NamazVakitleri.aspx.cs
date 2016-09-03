using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class NamazVakitleri : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UlkeSeosuzHaliGetir("");


            tarihBaslik.InnerText = DateTime.Now.ToLongDateString();
            IList<string> segmentler = Request.GetFriendlyUrlSegments();
            if (segmentler.Count > 0)
            {
                if (segmentler.Count == 2)
                {
                    string ulkeSeo = segmentler[0].ToString();
                    string sehirSeo = segmentler[1].ToString();

                    string ulkeNormal = UlkeSeosuzHaliGetir(ulkeSeo);
                    string sehirNormal = SehirSeosuzHaliGetir(ulkeSeo, sehirSeo);

                    hfUlke.Value = ulkeNormal;
                    hfSehir.Value = sehirNormal;

                    namazVaktiBaslik.InnerText = ulkeNormal + " - " + sehirNormal;
                    TabloOlustur(ulkeNormal, sehirNormal);
                    DescriptionTanimla(ulkeNormal, sehirNormal);
                }
                else if (segmentler.Count == 3)
                {
                    string ulkeSeo = segmentler[0].ToString();
                    string sehirSeo = segmentler[1].ToString();
                    string ilceSeo = segmentler[2].ToString();

                    string ulkeNormal = UlkeSeosuzHaliGetir(ulkeSeo);
                    string sehirNormal = SehirSeosuzHaliGetir(ulkeSeo, sehirSeo);
                    string ilceNormal = IlceSeosuzHaliGetir(ulkeSeo, sehirSeo, ilceSeo);

                    hfUlke.Value = ulkeNormal;
                    hfSehir.Value = sehirNormal;
                    hfIlce.Value = ilceNormal;

                    namazVaktiBaslik.InnerText = ulkeNormal + " - " + sehirNormal + " - " + ilceNormal;
                    TabloOlustur(ulkeNormal, sehirNormal, ilceNormal);
                    DescriptionTanimla(ulkeNormal, sehirNormal, ilceNormal);
                }
                else
                {
                    HataMesajGoster("Hatalı arama!");
                }
            }
            else
            {
                //string ulke = "ABD";
                //string sehir = "ALABAMA";
                //string ilce = "AUBURN";
                //string url = "/namaz-vakitleri/NamazVakitleri/" + UrlSEO(ulke) + "/" + UrlSEO(sehir) + "/" + UrlSEO(ilce);

                string ulke = "AFGANISTAN";
                string sehir = "FARAH";
                string ilce = "AUBURN";
                string url = "/namaz-vakitleri/NamazVakitleri/" + UrlSEO(ulke) + "/" + UrlSEO(sehir);

                Response.Redirect(url, true);
            }
        }
        catch (Exception ex)
        {
            HataMesajGoster(ex.Message);
        }
    }

    public string UlkeSeosuzHaliGetir(string ulkeSeolu)
    {
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlTextReader("https://www.netdata.com/XML/021030f1?$where=dc_Ulke_Kucuk_Harf=" + ulkeSeolu));

        //ds.ReadXml(new XmlTextReader("https://www.netdata.com/XML/021030f1"));

        //var dv = ds.Tables[0].DefaultView;
        //dv.RowFilter = "dc_Ulke_Kucuk_Harf = 'abd'";
        //var dtYeni = dv.ToTable();
        //DataSet dsYeni = new DataSet();
        //dsYeni.Tables.Add(dtYeni);

        //string aaa = "";

        //ds.Tables[0].DefaultView.RowFilter = "dc_Ulke_Kucuk_Harf='abd'";

        return ds.Tables[0].Rows[0]["dc_Ulke"].ToString();
    }
    public string SehirSeosuzHaliGetir(string ulkeSeolu, string sehirSeolu)
    {
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlTextReader("http://www.netdata.com/XML/f9e1b7e9?$where=dc_Ulke_Kucuk_Harf=" + ulkeSeolu + "[and]dc_Sehir_Kucuk_Harf=" + sehirSeolu));
        return ds.Tables[0].Rows[0]["dc_Sehir"].ToString();
    }
    public string IlceSeosuzHaliGetir(string ulkeSeolu, string sehirSeolu, string ilceSeolu)
    {
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlTextReader("http://www.netdata.com/XML/52a9b13e?$where=dc_Ulke_Kucuk_Harf=" + ulkeSeolu + "[and]dc_Sehir_Kucuk_Harf=" + sehirSeolu + "[and]dc_Ilce_Kucuk_Harf=" + ilceSeolu));
        return ds.Tables[0].Rows[0]["dc_Ilce"].ToString();
    }

    [WebMethod]
    public static string UlkeleriYukle()
    {
        dynamic sonuc = new ExpandoObject();
        try
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader("https://www.netdata.com/XML/021030f1"));
            DataView dv = new DataView(ds.Tables[0]);
            DataTable dt = dv.ToTable(true, new string[] { "dc_Ulke" });

            sb.Append("<option value=0>Ülke Seçiniz...</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ulke = dt.Rows[i]["dc_Ulke"].ToString();
                sb.Append("<option>" + ulke + "</option>");
            }
            sonuc.Hata = "";
            sonuc.Ulkeler = sb.ToString();
        }
        catch (Exception ex)
        {
            sonuc.Hata = ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(sonuc);
    }

    [WebMethod]
    public static string SehirleriYukle(string ulke)
    {
        dynamic sonuc = new ExpandoObject();
        try
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader("http://www.netdata.com/XML/f9e1b7e9?$where=dc_Ulke=" + ulke));
            DataView dv = new DataView(ds.Tables[0]);
            DataTable dt = dv.ToTable(true, new string[] { "dc_Sehir" });

            sb.Append("<option value=0>Şehir Seçiniz...</option>");

            if (IlcelerAktifMi(ulke) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sehir = dt.Rows[i]["dc_Sehir"].ToString();
                    sb.Append("<option>" + sehir + "</option>");
                    sb2.Append("<li><a href='/namaz-vakitleri/NamazVakitleri/" + UrlSEO(ulke) + "/" + UrlSEO(sehir) + "'>" + sehir + " Namaz Vakitleri</a></li>");
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sehir = dt.Rows[i]["dc_Sehir"].ToString();
                    sb.Append("<option>" + sehir + "</option>");
                }
            }

            sonuc.Hata = "";
            sonuc.Sehirler = sb.ToString();
            sonuc.SagTarafIller = sb2.ToString();
        }
        catch (Exception ex)
        {
            sonuc.Hata = ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(sonuc);
    }

    [WebMethod]
    public static string IlceleriYukle(string ulke, string sehir)
    {
        dynamic sonuc = new ExpandoObject();
        try
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader("http://www.netdata.com/XML/52a9b13e?$where=dc_Ulke=" + ulke + "[and]dc_Sehir=" + sehir));
            DataView dv = new DataView(ds.Tables[0]);
            DataTable dt = dv.ToTable(true, new string[] { "dc_Ilce" });

            sb.Append("<option value=0>Şehir Seçiniz...</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ilce = dt.Rows[i]["dc_Ilce"].ToString();
                sb.Append("<option>" + ilce + "</option>");
                sb2.Append("<li><a href='/namaz-vakitleri/NamazVakitleri/" + UrlSEO(ulke) + "/" + UrlSEO(sehir) + "/" + UrlSEO(ilce) + "'>" + ilce + " Namaz Vakitleri</a></li>");
            }

            sonuc.Hata = "";
            sonuc.Ilceler = sb.ToString();
            sonuc.SagTarafIller = sb2.ToString();
        }
        catch (Exception ex)
        {
            sonuc.Hata = ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(sonuc);
    }

    public static bool IlcelerAktifMi(string ulke)
    {
        if (ulke == "ABD" || ulke == "KANADA" || ulke == "TÜRKİYE")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TabloOlustur(string ulke, string sehir, string ilce = "")
    {
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();
        string hata = "";
        try
        {
            string tarih = DateTime.Now.ToString("dd.MM.yyyy");
            string link = "";
            if (ilce == "")
                link = "http://www.netdata.com/XML/f44daff7?$where=dc_Tarih=" + tarih + "[and]dc_Ulke=" + ulke + "[and]dc_Sehir=" + sehir;
            else
                link = "http://www.netdata.com/XML/f44daff7?$where=dc_Tarih=" + tarih + "[and]dc_Ulke=" + ulke + "[and]dc_Sehir=" + sehir + "[and]dc_Ilce=" + ilce;

            ds.ReadXml(new XmlTextReader(link));

            try
            {
                if (ds.Tables[0].Rows[0]["Exception"].ToString() != "")
                {
                    hata = "Hatalı Arama!";
                }
            }
            catch (Exception)
            { }

            sb.Append(@"<div class='col-xs-12 col-sm-8 col-sm-offset-2'><div class='panel panel-primary text-center'>
                            <div class='panel-heading'>
                                <label>Namaz Vakitleri</label>
                            </div>
                            <div class='panel-body'>");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sb.Append("<div class='row form-group'><div class='col-xs-12'><b>İmsak : </b>" + ds.Tables[0].Rows[i]["dc_Imsak"] + "</div></div>");
                sb.Append("<div class='row form-group'><div class='col-xs-12'><b>Güneş : </b>" + ds.Tables[0].Rows[i]["dc_Gunes"] + "</div></div>");
                sb.Append("<div class='row form-group'><div class='col-xs-12'><b>Öğle : </b>" + ds.Tables[0].Rows[i]["dc_Ogle"] + "</div></div>");
                sb.Append("<div class='row form-group'><div class='col-xs-12'><b>İkindi : </b>" + ds.Tables[0].Rows[i]["dc_Ikindi"] + "</div></div>");
                sb.Append("<div class='row form-group'><div class='col-xs-12'><b>Akşam : </b>" + ds.Tables[0].Rows[i]["dc_Aksam"] + "</div></div>");
                sb.Append("<div class='row form-group'><div class='col-xs-12'><b>Yatsı : </b>" + ds.Tables[0].Rows[i]["dc_Yatsi"] + "</div></div>");
                sb.Append("<div class='row form-group'><div class='col-xs-12'><b>Kıble : </b>" + ds.Tables[0].Rows[i]["dc_Kible"] + "</div></div>");
            }
            sb.Append("</div></div></div>");
            namazVakitleriList.InnerHtml = sb.ToString();
        }
        catch (Exception)
        {
            HataMesajGoster(hata);
        }
    }


    public static string UrlSEO(string Text)
    {
        System.Globalization.CultureInfo cui = new System.Globalization.CultureInfo("en-US");

        string strReturn = System.Net.WebUtility.HtmlDecode(Text.Trim());
        strReturn = strReturn.Replace("ğ", "g");
        strReturn = strReturn.Replace("Ğ", "g");
        strReturn = strReturn.Replace("ü", "u");
        strReturn = strReturn.Replace("Ü", "u");
        strReturn = strReturn.Replace("ş", "s");
        strReturn = strReturn.Replace("Ş", "s");
        strReturn = strReturn.Replace("ı", "i");
        strReturn = strReturn.Replace("İ", "i");
        strReturn = strReturn.Replace("ö", "o");
        strReturn = strReturn.Replace("Ö", "o");
        strReturn = strReturn.Replace("ç", "c");
        strReturn = strReturn.Replace("Ç", "c");
        strReturn = strReturn.Replace(" - ", "+");
        strReturn = strReturn.Replace("-", "+");
        strReturn = strReturn.Replace(" ", "+");
        strReturn = strReturn.Trim();
        strReturn = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9+]").Replace(strReturn, "");
        strReturn = strReturn.Trim();
        strReturn = strReturn.Replace("+", "-");
        return strReturn.ToLower(cui);
    }

    public void HataMesajGoster(string hata)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "Javascript",
              "bootbox.alert(\"" + hata + "\");", true);
    }

    [WebMethod]
    public static string AramaUrlDon(string ulke, string sehir, string ilce)
    {
        if (ilce != "")
        {
            return "/namaz-vakitleri/NamazVakitleri/" + UrlSEO(ulke) + "/" + UrlSEO(sehir) + "/" + UrlSEO(ilce);
        }
        else
        {
            return "/namaz-vakitleri/NamazVakitleri/" + UrlSEO(ulke) + "/" + UrlSEO(sehir);
        }
    }

    public void DescriptionTanimla(string ulke, string sehir, string ilce = "")
    {
        string desc = "";
        string keyw = "";
        if (ilce == "")
        {
            Page.Title = ulke + " - " + sehir + " Namaz Vakitleri";
            desc = ulke + " ülkesi " + sehir + " şehri namaz vakitlerinin gösteren web sitedir.";
            keyw = ulke + " namaz vakitleri, " + sehir + " namaz vakitleri, " + ulke + " ezan vakitleri, " + sehir + " ezan vakitleri, " + sehir + " şehri namaz vakitleri, "  + sehir + " şehri ezan vakitleri";
        }
        else
        {
            Page.Title = ulke + " - " + sehir + " - " + ilce + " Namaz Vakitleri";
            if (ulke == "ABD" || ulke == "KANADA")
            {
                desc = ulke + " ülkesi " + sehir + " eyaleti " + ilce + " şehri namaz vakitlerinin gösteren web sitedir.";
                keyw = ulke + " namaz vakitleri, " + sehir + " namaz vakitleri, " + ilce + " namaz vakitleri, " + ulke + " ezan vakitleri, " + sehir + " ezan vakitleri, " + ilce + " ezan vakitleri, " + ilce + " şehri namaz vakitleri, " + sehir + " eyaleti namaz vakitleri, " + ilce + " şehri ezan vakitleri";
            }
            else
            {
                desc = ulke + " ülkesi " + sehir + " şehri " + ilce + " ilçesi namaz vakitlerinin gösteren web sitedir.";
                keyw = ulke + " namaz vakitleri, " + sehir + " namaz vakitleri, " + ilce + " namaz vakitleri, " + ulke + " ezan vakitleri, " + sehir + " ezan vakitleri, " + ilce + " ezan vakitleri, " + ilce + " ilçesi namaz vakitleri, " + sehir + " şehri namaz vakitleri, " + sehir + " şehri ezan vakitleri";
            }
        }
        description.Attributes.Add("content", desc);
        keywords.Attributes.Add("content", keyw);
    }
}