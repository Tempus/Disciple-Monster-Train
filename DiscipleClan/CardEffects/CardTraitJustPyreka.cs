namespace DiscipleClan.CardEffects
{
    public class CardTraitJustPyreka : CardTraitState
    {
		public override bool GetIsPlayableFromHand(CardManager cardManager, RoomManager roomManager, out CardSelectionBehaviour.SelectionError selectionError)
		{
			int selectedRoom = roomManager.GetSelectedRoom();
			if (roomManager.GetRoom(selectedRoom) == roomManager.GetPyreRoom())
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
			if (roomManager.GetRoom(selectedRoom) == roomManager.GetPyreRoom())
			{
				selectionError = CardSelectionBehaviour.SelectionError.None;
				return true;
			}
			selectionError = CardSelectionBehaviour.SelectionError.Unplayable;
			return false;
		}

		public override string GetCardText()
		{
			return LocalizeTraitKey("MustPlayOnPyre_Title");
		}

		public override string GetCardTooltipId()
		{
			return "MustPlayOnPyre";
		}

		public override void CreateAdditionalTooltips(TooltipContainer tooltipContainer)
		{
			if (PreferencesManager.Instance.TipTooltipsEnabled)
			{
				TooltipUI tooltipUI = tooltipContainer.InstantiateTooltip("MustPlayOnPyre_Title");
				string body = "MustPlayOnPyre".Localize();
				tooltipUI?.Set(null, body);
			}
		}
	}
}
