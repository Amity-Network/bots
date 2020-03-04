using System;
using Discord;
using Discord.WebSocket;
using Nerva.Bots;
using Nerva.Bots.Helpers;
using Nerva.Bots.Plugin;
using Newtonsoft.Json;

namespace Atom.Commands
{
    [Command("cratex", "Get market info from Cratex.io")]
    public class Cratex : ICommand
    {
        public void Process(SocketUserMessage msg)
        {
            RequestData rd = Request.Http("https://cratex.io/api/v1/get_markets.php?market=XAM/BTC");
            if (!rd.IsError)
            {
                var json = JsonConvert.DeserializeObject<MarketInfo>(rd.ResultString);

                var em = new EmbedBuilder()
                .WithAuthor("Cratex Details", Globals.Client.CurrentUser.GetAvatarUrl())
                .WithDescription("The latest pricing from Cratex")
                .WithColor(Color.DarkGreen)
                .WithThumbnailUrl("https://getamitycoin.org/assets/cratex-logo.png");

                em.AddField("Volume", Math.Round(json.Volume, 5));
                em.AddField("Buy (Satoshis)", json.Ask * 100000000.0d, true);
                em.AddField("Sell (Satoshis)", json.Bid * 100000000.0d, true);
                //em.AddField("High", json.High * 100000000.0d, true);
                //em.AddField("Low", json.Low * 100000000.0d, true);

                DiscordResponse.Reply(msg, embed: em.Build());
            }
        }
    }
}