namespace Assets.Scripts.Core
{
    public enum ButtonType
    {
        NextPhaseButton,
        Undefined
    }

    public enum PlayerType
    {
        Player,
        Opponent
    }

    public enum ZoneType
    {
        Hand,
        BattleField
    }

    public enum ResourceType
    {
        Metal,
        Crystal,
        Deuterium
    }

    public enum CardType
    {
        Unit,
        Event
    }

    public enum ColorType
    {
        Normal,
        Selected,
        Targetable
    }

    public enum UnitType
    {
        None,
        Fighter,
        Battleship,
        Scoutship
    }

    public enum TargetType
    {
        Both,
        AnyUnit,
        AllyUnit,
        EnemyUnit
    }

    public enum SoundType
    {
        ButtonClick,
        BackgroundMusic,
        AccessMainFrame,
        LoginAuthorized,
        AccessArchives,
        ProgramActivated,
        ProgramTerminated,
        LaunchingActivated,
        LaunchingTerminated,
        AccessFiles,
        Synchronizing,
        PrepareHyperDrive,
        AccessDenied,
        ActiveHyperDrive,
        WrapDrive,
        InWrap
    }
}