public class BaseStrat {
    public enum MoveType {
        DEFAULT,
        STOP,
        FOLLOW,
        ALONE
    }

    public enum AttackType {
        DEFAULT,
        STOP,
        CLOSER,
        FARTHER
    }

    public enum CrewType {
        DEFAULT,
        FOCUS_GUNS,
        FOCUS_MOVE,
        FOCUS_REPAIR
    }

    public MoveType moveOrder = MoveType.DEFAULT;
    public AttackType attackOrder = AttackType.DEFAULT;
    public CrewType crewOrder = CrewType.DEFAULT;
}
