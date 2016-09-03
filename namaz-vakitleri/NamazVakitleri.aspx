<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NamazVakitleri.aspx.cs" Inherits="NamazVakitleri" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Namaz Vakitleri</title>
    <meta id="description" runat="server" name="description" content="" />
    <meta id="keywords" runat="server" name="keywords" content="" />
    <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=no" />
    <meta charset="utf-8">

    <link href="/namaz-vakitleri/CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="/namaz-vakitleri/CSS/bootstrap-select.min.css" rel="stylesheet" />
    <link href="/namaz-vakitleri/CSS/loader.css" rel="stylesheet" />
    <link href="/namaz-vakitleri/CSS/sitil.css" rel="stylesheet" />

    <script type="text/javascript" charset="windows-1254" src="/namaz-vakitleri/JS/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" charset="windows-1254" src="/namaz-vakitleri/JS/bootstrap.min.js"></script>
    <script type="text/javascript" charset="windows-1254" src="/namaz-vakitleri/JS/bootstrap-select.min.js"></script>
    <script type="text/javascript" charset="windows-1254" src="/namaz-vakitleri/JS/bootbox.min.js"></script>
    <script type="text/javascript" charset="windows-1254" src="/namaz-vakitleri/JS/main.js"></script>
    <script type="text/javascript">
        $(window).load(function () {
            $(".loaderStore").fadeOut("slow");
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="loaderStore">
            <div class='uil-reload-css'>
                <div></div>
            </div>
            <div class="divLoaderMesaj">
                <span class="spnLoaderMesajMetin"></span>
            </div>
        </div>

        <nav style="background: #4285F4; border-color: #1995dc;" class="navbar  navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a style="padding-top: 10px;" class="navbar-brand" href="http://www.netdata.com/">
                        <img src="/namaz-vakitleri/Img/logofornetsite2.png" alt="Netdata">
                    </a>
                </div>
                <div class="collapse navbar-collapse" id="myNavbar">
                    <ul class="nav navbar-nav">
                        <li class=""><a target="_blank" href="http://www.netdata.com/IFRAME/d944f71e"><span class="spnShowDatas">Örnek Verileri Göster</span></a></li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="container-fluid form-group">
            <div class="row">
                <div class="col-xs-12 col-sm-9">
                    <div class="row form-group">
                        <div class="col-xs-12 col-sm-4">
                            <div class="row text-center">
                                <img src="/namaz-vakitleri/Img/namaz.jpg" alt="." class="img-thumbnail img-responsive imgNamaz" />
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-8">
                            <div class="row form-group">
                                <div class="col-xs-12 text-center">
                                    <h1 class="text-danger">NAMAZ VAKİTLERİ</h1>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 text-center">
                                    <h2 id="namazVaktiBaslik" runat="server"></h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 text-center">
                                    <h2 id="tarihBaslik" runat="server"></h2>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-4">
                            <div class="panel panel-danger">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-12 form-group">
                                            <select id="txtUlke" class="selectpicker" data-live-search="true"></select>
                                            <asp:HiddenField ID="hfUlke" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 form-group">
                                            <select id="txtSehir" class="selectpicker" data-live-search="true">
                                                <option value="0">Şehir Seçiniz...</option>
                                            </select>
                                            <asp:HiddenField ID="hfSehir" runat="server" />
                                        </div>
                                    </div>
                                    <div id="divIlce" class="row" runat="server">
                                        <div class="col-xs-12">
                                            <select id="txtIlce" class="selectpicker" data-live-search="true">
                                                <option value="0">İlçe Seçiniz...</option>
                                            </select>
                                            <asp:HiddenField ID="hfIlce" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-footer text-center">
                                    <button type="button" class="btn btn-danger" onclick="Ara()">
                                        <span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                                        <span>ARA</span>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-8">
                            <div id="namazVakitleriList" runat="server" class="row text-center">
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-xs-12 col-sm-3 hidden-xs">
                    <div class="panel panel-danger">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <ul id="sagTarafIller"></ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
