using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConnectFour
{
    class Player
    {
        private string name;
        private bool ai;

        public Player (string name, bool AI)
        {
            this.name = name;
            this.ai = AI;
        }

        public string Name
        {
            get => name;
        }

        public bool AI
        {
            get => this.ai;
        }

        // AI chooses move to make - either based on last move made, or picks move at random
        public string AIMove(Game game)
        {
            Random random = new Random();
            string move = "";
            // If this is the first move, pick a random column in which to make a move
            if (game.Moves.Count == 0)
            {
                char[] xAxis = game.Board.GetXAxis(game.Board.Width);

                move = xAxis[random.Next(0, xAxis.Length)].ToString();
            }
            // If this is not the first move, pick a move based on the last move made
            else if (game.Moves.Count > 0)
            {
                // Get last move made
                KeyValuePair<Slot, Player> lastMove = game.Moves.Peek();
                char x = lastMove.Key.XCoordinate;
                int y = lastMove.Key.YCoordinate;

                // Get slots surrounding that move
                SortedList[] surroundingSlots = game.GetSurroundingSlots(x, y);

                // Establish the best move
                char bestMove = AIEstablishBestMove(game, surroundingSlots);
                move = bestMove.ToString();
            }
            return move;
        }

        // Establish the move(s) which will a) best prevent the opponent from winning
        // or b) build on a move previously made by the AI, and pick one at random. 
        // If neither of these types of move can be identified, pick a random move.
        public char AIEstablishBestMove(Game game, SortedList[] surroundingSlots)
        {
            Random random = new Random();
            char[] xAxis = game.Board.GetXAxis(game.Board.Width);
            // Establish who opponent is
            Player opponent = game.Moves.Peek().Value;

            // Establish all slots which could be filled by the AI in the next move
            Slot[] potentialNextMoves = new Slot[xAxis.Length];
            for (int i = 0; i < game.Board.GetXAxis(game.Board.Width).Length; i++)
            {
                Slot potentialMove = game.NextAvailableSlot(xAxis[i].ToString());
                if (potentialMove != null)
                {
                    potentialNextMoves[i]= potentialMove;
                }
            }

            // Play out each scenario to establish if any moves would prevent the opponent from winning
            for (int i = 0; i < potentialNextMoves.Length; i++)
            {
                if (potentialNextMoves[i] != null)
                {
                    Slot potentialMove = potentialNextMoves[i];
                    char x = potentialMove.XCoordinate;
                    int y = potentialMove.YCoordinate;

                    bool blocksOpponent = false;

                    game.Board.Slots[potentialMove.XCoordinate.ToString() + potentialMove.YCoordinate].Content = game.PlayerNumber(opponent.name);

                    SortedList[] surroundingNextMove = game.GetSurroundingSlots(x, y);

                    SortedList diagonalSlotsA = surroundingNextMove[0];
                    SortedList diagonalSlotsB = surroundingNextMove[1];
                    SortedList horizontalSlots = surroundingNextMove[2];
                    SortedList verticalSlots = surroundingNextMove[3];

                    if (game.FourInARow(diagonalSlotsA, opponent)
                        || game.FourInARow(diagonalSlotsB, opponent)
                        || game.FourInARow(horizontalSlots, opponent)
                        || game.FourInARow(verticalSlots, opponent))
                    {
                        blocksOpponent = true;
                    }

                    game.Board.Slots[potentialMove.XCoordinate.ToString() + potentialMove.YCoordinate].Content = 0;

                    if (blocksOpponent)
                    {
                        char blockingMove = x;
                        return blockingMove;
                    }
                }
            }

            // If no move would result in preventing the opponent from winning, 
            // then pick a move which would line up with a previous move made by the AI (AKA a "selfish" move)
            List<Slot> AIMoves = new List<Slot>();
            List<Slot> selfishMoves = new List<Slot>();
            string[] potentialCoordinates = new string[potentialNextMoves.Count()];
            int AIplayerNumber = game.PlayerNumber("Computer");

            // Turn collection of potential next slots into an array of string coordinates,
            // to allow for comparison with selfish move coordinates
            int count = 0;
            for (int i = 0; i < potentialNextMoves.Length; i++)
            {
                if (potentialNextMoves[i] != null)
                {
                    potentialCoordinates[count] = potentialNextMoves[i].XCoordinate.ToString() + potentialNextMoves[i].YCoordinate;
                    count++;
                }
            }

            foreach (Slot slot in game.Board.Slots.Values)
            {
                if (slot.Content == AIplayerNumber)
                {
                    AIMoves.Add(slot);
                }
            }

            // Go through each slot the AI has filled
            foreach (Slot move in AIMoves)
            {
                // Establish all the slots surrounding that slot
                SortedList[] slotsAroundAIMove = game.GetSurroundingSlots(move.XCoordinate, move.YCoordinate);
                for (int i = 0; i < slotsAroundAIMove.Length; i++)
                {
                    for (int j = 0; j < slotsAroundAIMove[i].Count; j++)
                    {
                        Slot slot = game.Board.Slots[slotsAroundAIMove[i].GetByIndex(j).ToString()];
                        string slotCoordinate = slot.XCoordinate.ToString() + slot.YCoordinate;
                        // Check the slot is empty and that it is one which be filled by the AI in the next move
                        if (game.Board.Slots[slotCoordinate].Content == 0
                            && potentialCoordinates.Contains(slotCoordinate))
                        {
                            // If it is, then add it to possible "selfish" moves
                            selfishMoves.Add(game.Board.Slots[slotCoordinate]);
                        }
                    }
                }
            }

            // If there are any selfish moves, pick one at random
            if (selfishMoves.Count > 0)
            {
                char selfishMove = selfishMoves.ElementAt(random.Next(0, selfishMoves.Count)).XCoordinate;
                return selfishMove;
            }

            // If the AI is not able to find a move which blocks the opponent from winning,
            // or a move which builds on one of its own previous moves,
            // it will pick a random (valid) slot
            else
            {
                bool validSlot = false;
                do
                {
                    char randomColumn = xAxis[random.Next(0, xAxis.Length)];

                    if (game.NextAvailableSlot(randomColumn.ToString()) != null)
                    {
                        validSlot = true;
                        return randomColumn;
                    }
                } while (!validSlot);
            }

            return ' ';
        }

        public int CountOpponentSlots(Game game, SortedList slotsToCheck)
        {
            // Establish who opponent is
            Player opponent = game.Moves.Peek().Value;

            // Count number of slots populated by opponent
            int opponentSlotCount = 0;
            for (int i = 0; i < slotsToCheck.Count; i++)
            {
                if (game.Board.Slots[slotsToCheck.GetByIndex(i).ToString()].Content == game.PlayerNumber(opponent.name))
                {
                    opponentSlotCount++;
                }
            }

            return opponentSlotCount;
        }
    }
}
