using System.Collections;
using UnityEngine;

namespace FS
{
    public class PlayerSnake
    {
        private BoardManager _boardManager;
        private BoardData _boardData;

        private CustomLinkedList<Character> characterList = new CustomLinkedList<Character>();

        public Character Head
        {
            get
            {
                if (characterList.Count == 0)
                    return null;
                return characterList.First.Value;
            }
        }

        public PlayerSnake(GameManager gameManager)
        {
            this._boardManager = gameManager.BoardManager;
            this._boardData = this._boardManager.BoardData;

        }

        public void AddCharacter(Character character)
        {
            if (characterList.Count == 0)
            {
                characterList.AddFirst(character);
            }
            else
            {
                Character last = characterList.Last.Value;
                characterList.AddLast(character);
                last.Next = character;
                character.Previous = last;

                character.CurrentPosition = last.CurrentPosition + last.LastDirection.Inverse().ToVector3();
                BoardObject newBoard = this._boardData.GetBoardObjectFromPosition(character.CurrentPosition);
                newBoard.SetObject(character);
            }
        }

        public IInteractable InteractNextObject(Direction dir)
        {
            if (characterList.First == null)
                return null;
            Character head = characterList.First.Value;
            Vector3 nextPos = head.CurrentPosition + dir.ToVector3();
            Vector3Int arrCoordinate = this._boardData.ConvertWorldPosToArrayPos(nextPos);
            BoardObject nextBoardObject = this._boardData.GetBoardObject(arrCoordinate.x, arrCoordinate.y);
            if (this._boardData.IsOutOfRange(arrCoordinate.x, arrCoordinate.y) || nextBoardObject.IsObstacle)
            {
                // remove head
                if (characterList.Count == 0)
                {
                    // dead.
                    return null;
                }
            }
            else if (nextBoardObject.IsCanInteract)
            {
                nextBoardObject.Interactable.Interact(head.gameObject);
                return nextBoardObject.Interactable;
            }
            return null;
        }
        public void PostInteractNextObject(IInteractable nextBoardObject)
        {
            Character head = characterList.First.Value;
            nextBoardObject.PostInteract(head.gameObject);
        }

        public void MoveToNewSlot(Direction dir)
        {
            Character head = characterList.First.Value;
            Character current = head;

            do
            {
                BoardObject boardObj = this._boardData.GetBoardObjectFromPosition(current.CurrentPosition);
                boardObj.Clear();

                if (current.Next)
                    current.Next.NextDirection = current.CurrentDirection;
                current.CurrentDirection = (current == head) ? dir : current.NextDirection;
                current.Move(current.CurrentDirection);

                BoardObject newBoard = this._boardData.GetBoardObjectFromPosition(current.CurrentPosition);
                newBoard.SetObject(current);

                current = current.Next;
            } while (current != null);
        }
    }
}
