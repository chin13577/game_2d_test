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

        public void SetOnUpdateHead(Action<Character> callback)
        {
            OnUpdateHead = callback;
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
            UpdateCharacterListToBoard();
            if (swapType == ESwapType.FORWARD)
            {
                characterList.FirstToLast();
            }
            else
            {
                characterList.LastToFirst();
            }

            SetCharacterListDirection(directionList);
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
            int index = 0;
            foreach (Character character in characterList)
            {
                character.CurrentDirection = directionList[index].CurrentDirection;
                character.NextDirection = directionList[index].NextDirection;
                index++;
            }
        }

        private void SwapPosition(ESwapType swapType)
        {
            if (swapType == ESwapType.FORWARD)
            {
                Vector3 lastPos = characterList.Last.CurrentPosition;
                foreach (Character character in characterList)
                {

                }
                //characterList.FirstToLast();
            }
            else
            {
                characterList.LastToFirst();
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
