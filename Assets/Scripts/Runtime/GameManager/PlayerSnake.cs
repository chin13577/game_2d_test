using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public enum ExecuteResult
    {
        INPUT_WRONG_DIRECTION,
        HIT_WALL,
        HIT_OWN_SNAKE_PART,
        HIT_ENEMY,
        PASS
    }

    public struct CharacterDirection
    {
        public Direction NextDirection;
        public Direction CurrentDirection;
    }

    public class PlayerSnake
    {
        public enum ESwapType
        {
            FORWARD,
            BACKWARD
        }

        private BoardManager _boardManager;
        private BoardData _boardData;

        private CustomLinkedList<Character> characterList = new CustomLinkedList<Character>();

        private Action<Character> OnUpdateHead;

        public Character Head
        {
            get
            {
                if (characterList.Count == 0)
                    return null;
                return characterList.First;
            }
        }
        public Character Tail
        {
            get
            {
                if (characterList.Count == 0)
                    return null;
                return characterList.Last;
            }
        }


        public int Count { get => characterList.Count; }

        public PlayerSnake(GameManager gameManager)
        {
            this._boardManager = gameManager.BoardManager;
            this._boardData = this._boardManager.BoardData;
        }

        public int GetHighestLevel()
        {
            int result = 1;
            for (int i = 0; i < this.characterList.Count; i++)
            {
                int level = this.characterList[i].Status.Level;
                if (level > result)
                {
                    result = level;
                }
            }
            return result;
        }

        public void SetOnUpdateHead(Action<Character> callback)
        {
            OnUpdateHead = callback;
        }

        public void ClearAllCharacterEventEmitter()
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                characterList[i].ClearAllEventEmitter();
            }
        }

        public void RemoveHead()
        {
            if (Count == 0)
                return;

            Character character = Head;
            SlotInfo currentSlot = this._boardData.GetSlotFromPosition(character.CurrentPosition);
            currentSlot.Clear();
            //TODO: implement pooling.
            character.gameObject.SetActive(false);
            characterList.RemoveFirst();

            OnUpdateHead?.Invoke(Head);
        }
        public void AddCharacter(Character character)
        {
            if (Count == 0)
            {
                characterList.AddFirst(character);
            }
            else
            {
                Character last = characterList.Last;
                characterList.AddLast(character);
                last.Next = character;
                character.Previous = last;

                character.CurrentPosition = last.CurrentPosition + last.LastDirection.Inverse().ToVector3();
                SlotInfo newSlot = this._boardData.GetSlotFromPosition(character.CurrentPosition);
                newSlot.SetObject(character);
            }
            character.SetOwnerSnake(this);
        }

        private bool IsCanMoveToDirection(Direction direction)
        {
            if (Count == 0)
                return false;
            else if (Count == 1)
                return true;
            else
            {
                Vector3 nextPos = Head.CurrentPosition + direction.ToVector3();
                if (Head.Next.CurrentPosition == nextPos)
                    return false;
                else
                    return true;
            }
        }
        public ExecuteResult ExecuteAndMove(Direction playerInputDir)
        {
            if (IsCanMoveToDirection(playerInputDir) == false)
            {
                return ExecuteResult.INPUT_WRONG_DIRECTION;
            }
            SlotInfo nextSlot = GetNextSlot(playerInputDir);
            if (nextSlot == null || nextSlot.IsObstacle)
            {
                Direction dir = Head.CurrentDirection;
                RemoveHead();
                MoveToNewSlot(dir);
                return ExecuteResult.HIT_WALL;
            }
            else if (nextSlot.IsHasObject && nextSlot.Obj.Team == Team.ENEMY)
            {
                return ExecuteResult.HIT_ENEMY;
            }
            else if (nextSlot.IsCanInteract)
            {
                if (IsSnakePart(nextSlot.Obj))
                {
                    return ExecuteResult.HIT_OWN_SNAKE_PART;
                }
                Character head = characterList.First;
                IInteractable interactable = nextSlot.Interactable;
                interactable.Interact(head.gameObject);

                MoveToNewSlot(playerInputDir);
                interactable.PostInteract(head.gameObject);
                return ExecuteResult.PASS;
            }
            else
            {
                MoveToNewSlot(playerInputDir);
                return ExecuteResult.PASS;
            }
        }

        public void SwapCharacter(ESwapType swapType)
        {
            if (Count <= 1)
                return;

            List<CharacterDirection> directionList = GetCharacterDirectionList();
            SwapPosition(swapType);
            SetRelation(swapType);
            UpdateCharacterListToBoard();

            SetCharacterListDirection(directionList);
            OnUpdateHead?.Invoke(Head);
        }

        private void SetRelation(ESwapType swapType)
        {
            if (swapType == ESwapType.FORWARD)
                characterList.FirstToLast();
            else
                characterList.LastToFirst();

            Head.Previous = null;
            for (int i = 1; i < characterList.Count; i++)
            {
                characterList[i].Previous = characterList[i - 1];
                characterList[i - 1].Next = characterList[i];
            }
            Tail.Next = null;
        }

        private void UpdateCharacterListToBoard()
        {
            foreach (Character character in characterList)
            {
                SlotInfo slot = this._boardData.GetSlotFromPosition(character.CurrentPosition);
                slot.SetObject(character);
            }
        }

        private List<CharacterDirection> GetCharacterDirectionList()
        {
            List<CharacterDirection> result = new List<CharacterDirection>();
            foreach (Character character in characterList)
            {
                result.Add(new CharacterDirection()
                {
                    CurrentDirection = character.CurrentDirection,
                    NextDirection = character.NextDirection
                });
            }
            return result;
        }

        private void SetCharacterListDirection(List<CharacterDirection> directionList)
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                characterList[i].CurrentDirection = directionList[i].CurrentDirection;
                characterList[i].NextDirection = directionList[i].NextDirection;
            }
        }

        private void SwapPosition(ESwapType swapType)
        {
            if (swapType == ESwapType.FORWARD)
            {
                Vector3 lastPos = characterList[characterList.Count - 1].CurrentPosition;
                for (int i = characterList.Count - 1; i >= 0; i--)
                {
                    int previousIndex = i - 1;
                    if (previousIndex >= 0)
                    {
                        characterList[i].CurrentPosition = characterList[previousIndex].CurrentPosition;
                    }
                }
                characterList[0].CurrentPosition = lastPos;
            }
            else
            {
                Vector3 firstPos = characterList[0].CurrentPosition;
                for (int i = 0; i < characterList.Count; i++)
                {
                    int nextIndex = i + 1;
                    if (nextIndex < characterList.Count)
                    {
                        characterList[i].CurrentPosition = characterList[nextIndex].CurrentPosition;
                    }
                }
                characterList[characterList.Count - 1].CurrentPosition = firstPos;
            }
        }


        private bool IsSnakePart(ISlotInfo obj)
        {
            if (obj.Team == Team.PLAYER)
            {
                Character character = obj.gameObject.GetComponent<Character>();
                if (character != null)
                    return characterList.Contains(character);
                return false;
            }
            else
            {
                return false;
            }
        }

        public SlotInfo GetNextSlot(Direction dir)
        {
            Character head = characterList.First;
            Vector3 nextPos = head.CurrentPosition + dir.ToVector3();
            return this._boardData.GetSlotFromPosition(nextPos);
        }

        public void MoveToNewSlot(Direction dir)
        {
            if (characterList.Count == 0)
                return;
            Character head = characterList.First;
            Character current = head;

            do
            {
                SlotInfo boardObj = this._boardData.GetSlotFromPosition(current.CurrentPosition);
                boardObj.Clear();

                if (current.Next)
                    current.Next.NextDirection = current.CurrentDirection;
                current.CurrentDirection = (current == head) ? dir : current.NextDirection;
                current.Move(current.CurrentDirection);

                SlotInfo newBoard = this._boardData.GetSlotFromPosition(current.CurrentPosition);
                newBoard.SetObject(current);

                current = current.Next;
            } while (current != null);
        }

    }
}
