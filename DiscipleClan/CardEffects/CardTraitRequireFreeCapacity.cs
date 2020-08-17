namespace DiscipleClan.CardEffects
{
    public class CardTraitRequireFreeCapacity : CardTraitState
    {
		public override bool GetIsPlayableFromHand(CardManager cardManager, RoomManager roomManager, out CardSelectionBehaviour.SelectionError selectionError)
		{
			int selectedRoom = roomManager.GetSelectedRoom();
			var capInfo = roomManager.GetRoom(selectedRoom).GetCapacityInfo(Team.Type.Monsters);
			if (capInfo.max >= capInfo.count + GetParamInt())
			{
				selectionError = CardSelectionBehaviour.SelectionError.None;
				return true;
			}
			selectionError = CardSelectionBehaviour.SelectionError.Unplayable;
			return false;
		}

		public override bool GetIsPlayableFromPlay(CardManager cardManager, RoomManager roomManager, out CardSelectionBehaviour.SelectionError selectionError)
		{
			int selectedRoom = roomManager.GetSelectedRoom();
			var capInfo = roomManager.GetRoom(selectedRoom).GetCapacityInfo(Team.Type.Monsters);
			if (capInfo.max >= capInfo.count + GetParamInt())
			{
				selectionError = CardSelectionBehaviour.SelectionError.None;
				return true;
			}
			selectionError = CardSelectionBehaviour.SelectionError.Unplayable;
			return false;
		}

		public override string GetCardText()
		{
			return "RequireFreeCapacity_Title".Localize(new LocalizedIntegers(GetParamInt()));
		}

		public override string GetCardTooltipId()
		{
			return "RequireFreeCapacity";
		}

		public override void CreateAdditionalTooltips(TooltipContainer tooltipContainer)
		{
			if (PreferencesManager.Instance.TipTooltipsEnabled)
			{
				TooltipUI tooltipUI = tooltipContainer.InstantiateTooltip("RequireFreeCapacity_Title");
				string body = "RequireFreeCapacity".Localize(new LocalizedIntegers(GetParamInt()));
				tooltipUI?.Set(null, body);
			}
		}
	}
}
