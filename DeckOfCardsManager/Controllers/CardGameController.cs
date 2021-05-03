using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeckOfCardsManager.Models;

namespace DeckOfCardsManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardGameController : ControllerBase
    {
        private static Dictionary<Guid, Game> _games = null;

        private Dictionary<Guid, Game> games
        {
            get
            {
                if (_games == null)
                {
                    _games = new Dictionary<Guid, Game>();
                }
                return _games;
            }
        }

        private readonly ILogger<CardGameController> _logger;

        public CardGameController(ILogger<CardGameController> logger)
        {
            _logger = logger;
        }

        [HttpGet("createGame")]
        public ActionResult<Guid> createGame([FromQuery]String name, [FromQuery] int numberOfPlayers, [FromQuery] Boolean aceHigh = false)
        {
            lock (games)
            {

                Game newGame = new Game(name, numberOfPlayers, aceHigh);

                games.Add(newGame.id, newGame);

                return newGame.id;
            }
        }

        [HttpGet("addPlayer")]
        public ActionResult<Guid> addPlayer([FromQuery] Guid gameId, [FromQuery] String name)
        {
            lock (games)
            {
                Game thisGame = games[gameId];

                thisGame.addPlayer(name);

                return thisGame.players[thisGame.players.Count - 1].id;
            }
        }

        [HttpGet("getTopDiscard")]
        public ActionResult<String> getTopDiscard([FromQuery] Guid gameId)
        {
            String card = "";
            lock(games)
            {
                card = games[gameId].discardPile[0].ToString();
            }

            return card;
        }

        [HttpGet("getGame")]
        public ActionResult<Game> getGame([FromQuery] Guid gameId)
        { 
            lock (games)
            {
                return games[gameId];
            }
        }

        [HttpGet("dealGame")]
        public ActionResult<int> dealGame([FromQuery] Guid gameId, int numberOfCards)
        {
            lock (games)
            {
                games[gameId].deal(numberOfCards);

                return games[gameId].drawPile.Count;
            }
        }

        [HttpGet("performTurn")]
        public ActionResult<String> performTurn([FromQuery] Guid gameId, [FromQuery] Guid playerId, [FromQuery] int drawnCards, [FromQuery] String[] discardedCards = null, [FromQuery] Boolean reverse = false)
        {
            lock (games)
            {
                Game thisGame = games[gameId];

                if(thisGame.currentPlayer.id == playerId)
                {
                    List<Card> discards = new List<Card>(discardedCards.Length);
                    foreach(String discard in discardedCards)
                    {
                        discards.Add(new Card(discard.Substring(1, 1), discard.Substring(0, 1)));
                    }
                    Boolean allDiscardsFound = true;
                    foreach(Card discard in discards)
                    {
                        if(!thisGame.players[thisGame.currentPlayerNumber].hand.Contains(discard))
                        {
                            allDiscardsFound = false;
                            break;
                        }
                    }
                    if (allDiscardsFound)
                    {
                        thisGame.turn(drawnCards, discards, reverse);
                    }
                }

                return "Success";
            }
        }
    }
}
