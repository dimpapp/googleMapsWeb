<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map.aspx.cs" Inherits="googleMapsWeb.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Google Map</title>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.2/themes/base/jquery-ui.css" /> 
    <style type="text/css">
    .style1 {background-color:#ffffff;font-weight:bold;border:2px #006699 solid;color:red}
    </style>
    <script src="http://code.jquery.com/jquery-latest.js"></script>
<script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js"></script>    
    <script type="text/javascript" src='http://maps.google.com/maps/api/js?sensor=false&language=en&key=AIzaSyCNcm4kWLPQm_PTffMO7umfD03p5SG4coc'></script>
    
    <script src="scripts/google.js"></script>
    <script src="scripts/jquery.js"></script>
    
    <script src="scripts/markerclusterer.js"></script>

    
</head>
<body>
    <form id="form1" runat="server">
        <table align="center" bgcolor="lightblue" cellspacing=3 cellpadding=3 style="border:1px solid navy;width:1200px;">
            <tr>
                <td colspan="2" align="center" valign=bottom>
                    <input type="button" id="mapAllBtn" value="Show all tweets" />
                    <input type="button" id="mapWordsBtn" value="Show popular words" />
                    <input type="button" id="mapWordsBtnDay" value="Show popular words per day" />
                    <input type="button" id="mapHashsBtn" value="Show popular hashtags" />
                    <input type="button" id="mapHashsBtnDay" value="Show popular hashtags per day" />
                    <br />
                    <div id="popularDay" style="padding-top:5px;"></div>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <span style="font-weight:normal;font-size:14px;color:red;font-family:Arial, Helvetica, sans-serif">Δημοφιλέστερα hashtags</span>:
                    <div style="height:8px;"><br /></div>
                    <asp:DropDownList runat="server" ID="hashtags" Width="200"></asp:DropDownList>
                    <input type="button" id="buttonFindit" value="Find it" />
                    <br />
                    <span style="font-weight:normal;font-size:14px;color:red;font-family:Arial, Helvetica, sans-serif">Περισσότερα retweets</span>:
                    <div style="height:8px;"><br /></div>
                    <asp:DropDownList runat="server" ID="retweets" Width="200"></asp:DropDownList>
                    <input type="button" id="buttonFindReTweets" value="Find it" />

                    <p align="left">
                    <div id="start"></div>
                    <div id="end"></div>
                        </p>
                    <input type="button" id="buttonGo" value="Go" style="display:none"/>

                    <input type="button" id="buttonRe" value="Go" style="display:none" />

                    <p align="left">
                    <div id="twitDate"></div>
                        <div id="twitNumber"></div>
                        <div id="twitTotal"></div>
                        </p>
                </td>
                <td align="center" rowspan="2" >

                    <div id="includeMap" style=" width:800px; height:500px;border:1px solid #9cacb2" >
 <div id="mapi" style="width:100%;height:100%;" ></div>
 </div>
                </td>
                </tr>
            <tr>
                <td align="center" valign="top">
                    
                </td>
            </tr>
        </table>

        <input type="hidden" id="day1_1" />
        <input type="hidden" id="day1_2" />
        <input type="hidden" id="day1_3" />
        <input type="hidden" id="day2_1" />
        <input type="hidden" id="day2_2" />
        <input type="hidden" id="day2_3" />
    </form>



</body>
</html>
