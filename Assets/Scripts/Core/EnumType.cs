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
        Scoutship,
        Robot
    }

    public enum TargetType
    {
        Both,
        AnyUnit,
        AllyUnit,
        EnemyUnit
    }

    public enum AudioClipType
    {
        ButtonClick = 0,
        Greetings = 1,
        AccessMainFrame = 2,
        LoginAuthorized = 3,
        AccessArchives = 4,
        ProgramActivated = 5,
        ProgramTerminated = 6,
        LaunchingActivated = 7,
        LaunchingTerminated = 8,
        AccessFiles = 9,
        Synchronizing = 10,
        PrepareHyperDrive = 11,
        AccessDenied = 12,
        ActiveHyperDrive = 13,
        WrapDrive = 14
    }
}