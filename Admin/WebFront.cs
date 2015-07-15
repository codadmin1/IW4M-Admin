﻿#define ENABLED_CRAP_CODE_THAT_NEEDS_TO_BE_REWRITTEN
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kayak;
using Kayak.Http;
using System.Net;


#if ENABLED_CRAP_CODE_THAT_NEEDS_TO_BE_REWRITTEN
namespace IW4MAdmin_Web
{
    class Client
    {
        public Client ( WebFront.Page req, int cur, IDictionary<String, String> inc, String D, IW4MAdmin.Player P)
        {
            requestedPage = req;
            requestedPageNumber = cur;
            requestOrigin = inc;
            requestData = D;
            playerRequesting = P;
        }

        public WebFront.Page requestedPage { get; private set; }
        public int requestedPageNumber { get; private set; }
        public IDictionary<String, String> requestOrigin { get; private set; }
        public String requestData { get; private set; }
        public IW4MAdmin.Player playerRequesting { get; private set; }

    }

    class WebFront
    {
        private IW4MAdmin.Server[] Servers;

        public enum Page
        {
            main,
            stats,
            bans,
            player
        }

        public WebFront()
        {
            Servers = IW4MAdmin.Program.getServers();
        }

        public void Init()
        {
            webSchedule = KayakScheduler.Factory.Create(new SchedulerDelegate());
            webServer = KayakServer.Factory.CreateHttp(new RequestDelegate(), webSchedule);
           
            using (webServer.Listen(new IPEndPoint(IPAddress.Any, 1624)))
            {
                // runs scheduler on calling thread. this method will block until
                // someone calls Stop() on the scheduler.
                webSchedule.Start();
            }
        }

        private IScheduler webSchedule;
        private IServer webServer;
    }

    static class Macro
    {
        static public String parsePagination(int server, int totalItems, int itemsPerPage, int currentPage, String Page)
        {
            StringBuilder output = new StringBuilder();

            output.Append("<div id=pages>");

            if ( currentPage > 0)
                output.AppendFormat("<a href=/{0}/{1}/?{2}>PREV</a>", server, currentPage - 1, Page);
            double totalPages = Math.Ceiling(((float)totalItems / itemsPerPage));
            output.Append("<span id=pagination>" + (currentPage + 1) + "/" + totalPages + "</span>");
            if ((currentPage + 1) < totalPages)
                output.AppendFormat("<a href=/{0}/{1}/?{2}>NEXT</a>", server, currentPage + 1, Page);
            output.Append("</div>");

            return output.ToString();
        }

        static public String parseMacros(String input, WebFront.Page Page, int server, int Pagination, bool logged, String Data)
        {
            StringBuilder buffer = new StringBuilder();
            IW4MAdmin.Server[] Servers= IW4MAdmin.Program.getServers();
            switch (input)
            {
                case "SERVERS":
                    int cycleFix = 0;
                    for (int i = 0; i < Servers.Count(); i++)
                    {
                        if (IW4MAdmin.Program.getServers()[i] == null)
                            continue;

                        StringBuilder players = new StringBuilder();
                        if (Servers[i].getClientNum() < 1)
                            players.Append("<h2>No Players</h2>");
                        else
                        {
                            int count = 0;
                            double currentPlayers = Servers[i].statusPlayers.Count;

                            foreach (IW4MAdmin.Player P in Servers[i].getPlayers())
                            {
                                if (P == null)
                                    continue;

                                if (count % 2 == 0)
                                {
                                    switch (cycleFix)
                                    {
                                        case 0:
                                            players.Append("<tr class='row-grey'>");
                                            cycleFix = 1;
                                            break;
                                        case 1:
                                            players.Append("<tr class='row-white'>");
                                            cycleFix = 0;
                                            break;
                                    }
                                }

                                players.AppendFormat("<td><a href='/{0}/{1}/userip/?player'>{2}</a></td>", i, P.getDBID(), IW4MAdmin.Utilities.nameHTMLFormatted(P));

                                if (count % 2 != 0)
                                {
                                    players.Append("</tr>");
                                }

                                count++;

                            }
                        }
                        buffer.AppendFormat(@"<table cellpadding=0 cellspacing=0 class=server>
                                                <tr>
                                                    <th class=server_title><span>{0}</span></th>
                                                    <th class=server_map><span>{1}</span></th>
                                                    <th class=server_players><span>{2}</span></th>
                                                    <th class=server_gametype><span>{3}</span></th>
                                                    <th><a href=/{4}/0/?stats>Stats</a></th>
                                                    <th><a href=/{4}/0/?bans>Bans</a></th>
                                                    <th><a class='history' href='/{4}/0/?playerhistory'>History</a></th>
                                                 </tr>
                                             </table>
                                             <table cellpadding='0' cellspacing='0' class='players'>
                                                    {5}
                                            </table>",
                                             Servers[i].getName(), Servers[i].getMap(), Servers[i].getClientNum() + "/" + Servers[i].getMaxClients(), IW4MAdmin.Utilities.gametypeLocalized(Servers[i].getGametype()), i, players.ToString());
                        buffer.AppendFormat("<div class='chatHistory' id='chatHistory_{0}'></div><script type='text/javascript'>$( document ).ready(function() {{ setInterval({1}loadChatMessages({0}, '#chatHistory_{0}'){1}, 2500); }});</script><div class='null' style='clear:both;'></div>", i, '\"');
                        if (Servers[i].getClientNum() > 0)
                            buffer.AppendFormat("<form class='chatOutFormat' action={1}javascript:chatRequest({0}, 'chatEntry_{0}'){1}><input class='chatFormat_text' type='text' placeholder='Enter a message...' id='chatEntry_{0}'/><input class='chatFormat_submit' type='submit'/></form>", i, '\"');
                        buffer.Append("<hr/>");
                    }
                    return buffer.ToString();
                case "TITLE":
                    return "IW4M Administration";
                case "BANS":
                    buffer.Append("<table cellspacing=0 class=bans>");
                    int totalBans = IW4MAdmin.Program.getServers()[0].Bans.Count;
                    int range;
                    int start = Pagination*30;
                    cycleFix = 0;

                    if (totalBans <= 30)
                        range = totalBans - 1;
                    else if ((totalBans - start) < 30)
                        range = (totalBans - start);
                    else
                        range = 30;

                    List<IW4MAdmin.Ban> Bans = new List<IW4MAdmin.Ban>();

                    if (totalBans > 0)
                        Bans = IW4MAdmin.Program.getServers()[0].Bans.GetRange(start, range).OrderByDescending(x => x.getTime()).ToList();
                    else
                        Bans.Add(new IW4MAdmin.Ban("No Bans", "0", "0", DateTime.Now, ""));


                    buffer.Append("<h1 style=margin-top: 0;>{{TIME}}</h1><hr /><tr><th>Name</th><th style=text-align:left;>Offense</th><th style=text-align:left;>Banned By</th><th style='width: 175px; text-align:right;padding-right: 80px;'>Time</th></tr>");

                    if (Bans[0] != null)
                        buffer = buffer.Replace("{{TIME}}", "From " + IW4MAdmin.Utilities.timePassed(Bans[0].getTime()) + " ago" + " &mdash; " + totalBans + " total");
             
                    for (int i = 0; i < Bans.Count; i++)
                    {
                        if (Bans[i] == null)
                            continue;

                        IW4MAdmin.Player P = IW4MAdmin.Program.getServers()[0].clientDB.getPlayer(Bans[i].getID(), -1);
                        IW4MAdmin.Player B = IW4MAdmin.Program.getServers()[0].clientDB.getPlayer(Bans[i].getBanner(), -1);

                        if (P == null)
                            P = new IW4MAdmin.Player("Unknown", "n/a", 0, 0, 0, "Unknown", 0, "");
                        if (B == null)
                            B = new IW4MAdmin.Player("Unknown", "n/a", 0, 0, 0, "Unknown", 0, "");

                        if (P.getLastO() == String.Empty)
                            P.LastOffense = "Evade";

                        if (P != null && B != null)
                        {
                            if (B.getID() == P.getID())
                                B.updateName("IW4MAdmin"); // shh it will all be over soon

                            String Prefix;
                            if (cycleFix % 2 == 0)
                                Prefix = "class=row-grey";
                            else
                                Prefix = "class=row-white";
                            String Link = "/" + server + "/" + P.getDBID() + "/userip/?player";
                            buffer.AppendFormat("<tr {4}><td><a href='{5}'>{0}</a></th><td style='border-left: 3px solid #bbb; text-align:left;'>{1}</td><td style='border-left: 3px solid #bbb;text-align:left;'>{2}</td><td style='width: 175px; text-align:right;'>{3}</td></tr></div>", P.getName(), P.getLastO(), IW4MAdmin.Utilities.nameHTMLFormatted(B), Bans[i].getWhen(), Prefix, Link);
                            cycleFix++;
                        }
                    }
                    buffer.Append("</table><hr/>");

                    buffer.Append(parsePagination(server, Servers[0].Bans.Count, 30, Pagination, "bans"));
                    return buffer.ToString();
                case "PAGE":
                    buffer.Append("<div id=pages>");              
                    return buffer.ToString();
                case "STATS":
                    int totalStats = Servers[server].statDB.totalStats();
                    buffer.Append("<h1 style='margin-top: 0;'>Starting at #{{TOP}}</h1><hr />");
                    buffer.Append("<table style='width:100%' cellspacing=0 class=stats>");
 
                    start = Pagination*30;
                    if (totalStats <= 30)
                        range = totalStats - 1;
                    else if ((totalStats - start) < 30 )
                        range = (totalStats - start);
                    else
                        range = 30;
                    List<IW4MAdmin.Stats> Stats = Servers[server].statDB.getMultipleStats(start, range).OrderByDescending(x => x.Skill).ToList();
                    buffer.Append("<tr><th style=text-align:left;>Name</th><th style=text-align:left;>Kills</th><th style=text-align:left;>Deaths</th><th style=text-align:left;>KDR</th><th style='width: 175px; text-align:right;'>Rating</th></tr>");
                    cycleFix = 0;
                    for (int i = 0; i < totalStats; i++)
                    {
                        if (i >= Stats.Count -1 || Stats[i] == null )
                            continue;

                        IW4MAdmin.Player P = Servers[server].clientDB.getPlayer(Stats[i].statIndex);

                        if (P == null)
                            continue;

                        P.stats = Stats[i];


                        if (P.stats != null)
                        {
                            String Prefix;
                            if (cycleFix % 2 == 0)
                                Prefix = "class=row-grey";
                            else
                                Prefix = "class=row-white";

                            String Link = "/" + server + "/" + P.getDBID() + "/userip/?player";
                            buffer.AppendFormat("<tr {5}><td><a href='{6}'>{0}</a></td><td style='border-left: 3px solid #bbb; text-align:left;'>{1}</td><td style='border-left: 3px solid #bbb;text-align:left;'>{2}</td><td style='border-left: 3px solid #bbb;text-align:left;'>{3}</td><td style='width: 175px; text-align:right;'>{4}</td></tr></div>", P.getName(), P.stats.Kills, P.stats.Deaths, P.stats.KDR, P.stats.Skill, Prefix, Link);
                            cycleFix++;
                        }
                    }
                    buffer.Append("</table><hr/>");
                    buffer.Append(parsePagination(server, totalStats, 30, Pagination, "stats"));
                    return buffer.ToString().Replace("{{TOP}}", (start + 1).ToString());
                case "PLAYER":
                    buffer.Append("<table class='player_info'><tr><th>Name</th><th>Aliases</th><th>IP</th><th>Rating</th><th>Level</th><th>Connections</th><th>Last Seen</th><th>Profile</th>");
                    List<IW4MAdmin.Player> matchingPlayers = new List<IW4MAdmin.Player>();

                    if (Data == null)
                        matchingPlayers.Add(Servers[server].clientDB.getPlayer(Pagination));
                    else
                    {
                        var alias = Servers[server].aliasDB.findPlayers(Data);

                        foreach (var a in alias)
                        {
                            var p = Servers[server].clientDB.getPlayer(a.getNumber());
                            if (p != null)
                            {
                                List<IW4MAdmin.Player> aliases = new List<IW4MAdmin.Player>();
                                Servers[server].getAliases(aliases, p);

                                foreach (var pa in aliases)
                                {
                                    if (!matchingPlayers.Exists(x => x.getDBID() == pa.getDBID()))
                                        matchingPlayers.Add(pa);
                                }
                            }
                        }
                    }

                    if (matchingPlayers == null)
                        buffer.Append("</table>");

                    else
                    {
                        foreach (IW4MAdmin.Player Player in matchingPlayers)
                        {
                            if (Player == null)
                                continue;
                            
                            buffer.Append("<tr>");
                            StringBuilder str = new StringBuilder();
                            List<IW4MAdmin.Player> aliases = new List<IW4MAdmin.Player>();
                            Servers[server].getAliases(aliases, Player);

                            foreach (IW4MAdmin.Player a in aliases)
                            {
                                if (Data != null)
                                {
                                    if (a.Alias.getNames().Exists(p => p.ToLower().Contains(Data.ToLower())) && a.getDBID() != Player.getDBID())
                                    {
                                         str.AppendFormat("<span>{0}</span><br/>", a.getName());
                                         break;
                                    }
                                }
                                else
                                    str.AppendFormat("<span>{0}</span><br/>", a.getName());
                            }

                            Player.stats = Servers[server].statDB.getStats(Player.getDBID());
                            String Rating = String.Empty;

                            if (Player.stats == null)
                                Rating = "Not Available";
                            else
                                Rating = Player.stats.Skill.ToString();

                            StringBuilder IPs = new StringBuilder();

                            if (logged)
                            {
                                foreach (IW4MAdmin.Player a in aliases)
                                {
                                    foreach (String ip in a.Alias.getIPS())
                                    {
                                        if (!IPs.ToString().Contains(ip))
                                            IPs.AppendFormat("<span>{0}</span><br/>", ip);
                                    }
                                }

                            }
                            else
                                IPs.Append("XXX.XXX.XXX.XXX");

                            Int64 forumID = 0;
                            if (Player.getID().Length == 16)
                            {
                                forumID = Int64.Parse(Player.getID().Substring(0, 16), NumberStyles.AllowHexSpecifier);
                                forumID = forumID - 76561197960265728;
                            }

                            String Screenshot = String.Empty;

                            if (logged)
                                Screenshot = String.Format("<a href='http://server.nbsclan.org/screen.php?id={0}&name={1}'><div style='background-image:url(http://server.nbsclan.org/shutter.png); width: 20px; height: 20px;float: right; position:relative; right: 21%; background-size: contain;'></div></a>", forumID, Player.getName());

                            buffer.AppendFormat("<td><a style='float: left;' href='{9}'>{0}</a>{10}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6} ago</td><td><a href='https://repziw4.de/memberlist.php?mode=viewprofile&u={7}'>{8}</a></td>", Player.getName(), str, IPs, Rating, IW4MAdmin.Utilities.nameHTMLFormatted(Player.getLevel()), Player.getConnections(), Player.getLastConnection(), forumID, Player.getName(), "/0/" + Player.getDBID() + "/userip/?player", Screenshot);
                            buffer.Append("</tr>");
                        }

                        buffer.Append("</table>");
                    }

                    return buffer.ToString();
                default:
                    return input;
            }
        }

        static public String findMacros(String input, Client C, int server)
        {
            String output = input;

            bool logged = IW4MAdmin.Program.getServers()[server].clientDB.getAdmins().Exists(player => player.getIP() == C.requestOrigin["Host"].Split(':')[0]);

            switch (C.requestedPage)
            {
                case WebFront.Page.main:
                    output = output.Replace("{{SERVERS}}", parseMacros("SERVERS", C.requestedPage, server, C.requestedPageNumber, logged, C.requestData));
                    break;
                case WebFront.Page.bans:
                    output = output.Replace("{{BANS}}", parseMacros("BANS", C.requestedPage, server, C.requestedPageNumber, logged, C.requestData));
                    break;
                case WebFront.Page.stats:
                    output = output.Replace("{{STATS}}", parseMacros("STATS", C.requestedPage, server, C.requestedPageNumber, logged, C.requestData));
                    break;
                case WebFront.Page.player:
                    output = output.Replace("{{PLAYER}}", parseMacros("PLAYER", C.requestedPage, server, C.requestedPageNumber, (C.playerRequesting.getLevel() > IW4MAdmin.Player.Permission.Flagged), C.requestData));
                    break;
            }

            output = output.Replace("{{TITLE}}", "IW4M Administration");
            output = output.Replace("{{VERSION}}", IW4MAdmin.Program.Version.ToString());

            return output;
        }
    }

    class SchedulerDelegate : ISchedulerDelegate
    {
        public void OnException(IScheduler scheduler, Exception e)
        {
            Console.WriteLine(e.InnerException.Message);
            Console.Write(e.InnerException);
            e.DebugStackTrace();
        }

        public void OnStop(IScheduler scheduler)
        {

        }
    }

    class RequestDelegate : IHttpRequestDelegate
    {
        public void OnRequest(HttpRequestHead request, IDataProducer requestBody, IHttpResponseDelegate response)
        {
            String type = "text/html";
            if (request.Uri.StartsWith("/"))
            {
                //Console.WriteLine("[WEBFRONT] Processing Request for " + request.Uri);             
                var body = String.Empty;

                if (request.Uri.StartsWith("/"))
                {
                    IW4MAdmin.file Header = new IW4MAdmin.file("webfront\\header.html");
                    var header = Header.getLines();
                    Header.Close();

                    IW4MAdmin.file Footer = new IW4MAdmin.file("webfront\\footer.html");
                    var footer = Footer.getLines();
                    Footer.Close();

                    String[] req = request.Path.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

                    int server = 0;
                    int page = 0;

                    if (req.Length > 1)
                    {
                        Int32.TryParse(req[0], out server);
                        Int32.TryParse(req[1], out page);
                    }

                    if (request.QueryString == "bans")
                    {
                        IW4MAdmin.file Bans = new IW4MAdmin.file("webfront\\bans.html");
                        var bans = Bans.getLines();
                        Bans.Close();
                        Client toSend = new Client(WebFront.Page.bans, page, request.Headers, null, null);
                        body = Macro.findMacros((header + bans + footer), toSend, server);
                    }

                    else if (request.QueryString == "stats")
                    {
                        IW4MAdmin.file Stats = new IW4MAdmin.file("webfront\\stats.html");
                        var stats = Stats.getLines();
                        Stats.Close();
                        Client toSend = new Client(WebFront.Page.stats, page, request.Headers, null, null);
                        body = Macro.findMacros(header + stats + footer, toSend, server);
                    }

                    else if (request.QueryString == "playerhistory")
                    {
                        //type = "text/plain";
                        StringBuilder test = new StringBuilder();
                        test.Append("<script type='text/javascript' src='//www.google.com/jsapi'></script><div id='chart_div'></div>");
                        test.Append("<script> var players = [");
                        int count = 1;
                        List<IW4MAdmin.pHistory> run = IW4MAdmin.Program.getServers()[server].playerHistory.ToList();
                        foreach (IW4MAdmin.pHistory i in run) //need to reverse for proper timeline
                        {
                            test.AppendFormat("[new Date({0}, {1}, {2}, {3}, {4}), {5}]", i.When.Year, i.When.Month - 1, i.When.Day, i.When.Hour, i.When.Minute, i.Players);
                            if (count < IW4MAdmin.Program.getServers()[server].playerHistory.Count)
                                test.Append(",\n");
                            count++;
                        }
                        test.Append("];\n");
                        test.Append("</script>");
                        IW4MAdmin.file Graph = new IW4MAdmin.file("webfront\\graph.html");
                        var graph = Graph.getLines();
                        Graph.Close();
                        body = test.ToString()  + graph ;
                    }

                    else if (request.QueryString == "player")
                    {
                        IW4MAdmin.file Player = new IW4MAdmin.file("webfront\\player.html");
                        var player = Player.getLines();
                        Player.Close();
                        String Data = null, IP = null, ID = null;
                        if (req.Length > 3)
                        {
                            ID = req[1];
                            IP = req[2];
                            Data = req[3];
                        }

                        else if (req.Length >2)
                        {
                            ID = req[1];
                            IP = req[2];
                        }

                        IW4MAdmin.Player P = IW4MAdmin.Program.getServers()[server].clientDB.getPlayer(IP);
                        if (P == null)
                            P = new IW4MAdmin.Player("Guest", "Guest", 0, 0);
                       // if (P.getLevel() > IW4MAdmin.Player.Permission.Flagged)
                        //    Console.WriteLine(P.getName() + " is authenticate");

                        Client toSend = new Client(WebFront.Page.player, page, request.Headers, Data, P);
                        body = Macro.findMacros(header + player + footer, toSend, server);
                    }

                    else if (request.QueryString == "chat")
                    {
                        StringBuilder chatMessages = new StringBuilder();
#if DEBUG
                      //  if (IW4MAdmin.Program.Servers[server].chatHistory.Count < 8)
                      //      IW4MAdmin.Program.Servers[server].chatHistory.Add(new IW4MAdmin.Chat(new IW4MAdmin.Player("TEST", "xuid", 0, 0), "TEST MESSAGE", DateTime.Now));
#endif
                        String IP, Text;
                        if (req.Length > 3)
                        {
                            IP = req[2];
                            Text = IW4MAdmin.Utilities.cleanChars(HttpUtility.UrlDecode(req[3]));
                        }

                        else
                        {
                            IP = null;
                            Text = null;
                        }

                        if (IP == null && IW4MAdmin.Program.getServers()[server].getClientNum() > 0)
                        {
                            chatMessages.Append("<table id='table_chatHistory'>");
                            foreach (IW4MAdmin.Chat Message in IW4MAdmin.Program.getServers()[server].chatHistory)
                                chatMessages.AppendFormat("<tr><td class='chat_name' style='text-align: left;'>{0}</td><td class='chat_message'>{1}</td><td class='chat_time' style='text-align: right;'>{2}</td></tr>", IW4MAdmin.Utilities.nameHTMLFormatted(Message.Origin), Message.Message, Message.timeString());
                            chatMessages.Append("</table>");
                            body = chatMessages.ToString();
                        }

                        else if (Text != null && Text.Length > 4)
                        {
                            IW4MAdmin.Player requestPlayer = IW4MAdmin.Program.getServers()[server].clientDB.getPlayer(IP);

                            if (requestPlayer != null)
                                IW4MAdmin.Program.getServers()[server].webChat(requestPlayer, Text);
                        }
                    }

                    else if (request.QueryString == "pubbans")
                    {
                        type = "text/plain";
                        StringBuilder banTXT = new StringBuilder();
                        banTXT.AppendFormat("===========================================\nIW4M ADMIN PUBLIC BAN LIST\nGENERATED {0}\nIP---GUID---REASON---TIME\n===========================================\n", DateTime.Now.ToString());
                        foreach (IW4MAdmin.Ban B in IW4MAdmin.Program.getServers()[0].Bans)
                        {
                            if (B.getIP() != null && B.getIP() != String.Empty && B.getReason() != null && B.getReason() != String.Empty)
                                banTXT.AppendFormat("{0}---{1}---{2}---{3}\n", B.getIP(), B.getID(), B.getReason().Trim(), Math.Round((B.getTime()-DateTime.MinValue).TotalSeconds, 0));
                        }
                        body = banTXT.ToString();
                    }

                    else
                    {
                        IW4MAdmin.file Main = new IW4MAdmin.file("webfront\\main.html");
                        var main = Main.getLines();
                        Main.Close();
                        Client toSend = new Client(WebFront.Page.main, page, request.Headers, null, null);
                        body = Macro.findMacros(header + main + footer, toSend, server);
                    }

                    //IW4MAdmin.Program.getServers()[server].Log.Write("Webfront processed request for " + request.Uri, IW4MAdmin.Log.Level.Debug);
                }

                var headers = new HttpResponseHead()
                {
                    Status = "200 OK",
                    Headers = new Dictionary<string, string>() 
                    {
                        { "Content-Type", type },
                        { "Content-Length", body.Length.ToString() },
                    }
                };

                response.OnResponse(headers, new BufferedProducer(body));
            }

            else
            {
                var responseBody = "The resource you requested ('" + request.Uri + "') could not be found.";
                var headers = new HttpResponseHead()
                {
                    Status = "404 Not Found",
                    Headers = new Dictionary<string, string>()
                    {
                        { "Content-Type", type },
                        { "Content-Length", responseBody.Length.ToString() }
                    }
                };
                var body = new BufferedProducer(responseBody);

                response.OnResponse(headers, body);
            }
        }

        class BufferedProducer : IDataProducer
        {
            ArraySegment<byte> data;

            public BufferedProducer(string data) : this(data, Encoding.UTF8) { }
            public BufferedProducer(string data, Encoding encoding) : this(encoding.GetBytes(data)) { }
            public BufferedProducer(byte[] data) : this(new ArraySegment<byte>(data)) { }
            public BufferedProducer(ArraySegment<byte> data)
            {
                this.data = data;
            }

            public IDisposable Connect(IDataConsumer channel)
            {
                // null continuation, consumer must swallow the data immediately.
                channel.OnData(data, null);
                channel.OnEnd();
                return null;
            }
        }

        class BufferedConsumer : IDataConsumer
        {
            List<ArraySegment<byte>> buffer = new List<ArraySegment<byte>>();
            Action<string> resultCallback;
            Action<Exception> errorCallback;

            public BufferedConsumer(Action<string> resultCallback, Action<Exception> errorCallback)
            {
                this.resultCallback = resultCallback;
                this.errorCallback = errorCallback;
            }
            public bool OnData(ArraySegment<byte> data, Action continuation)
            {
                // since we're just buffering, ignore the continuation. 
                buffer.Add(data);
                return false;
            }
            public void OnError(Exception error)
            {
                errorCallback(error);
            }

            public void OnEnd()
            {
                // turn the buffer into a string. 
                var str = buffer
                    .Select(b => Encoding.UTF8.GetString(b.Array, b.Offset, b.Count))
                    .Aggregate((result, next) => result + next);

                resultCallback(str);
            }
        } 
    }
 }
#endif