namespace Building.Additional
{
    public interface IBuildingJobStatus
    {
        bool isWorked { get; protected set; }


        void ChangeJobStatus(in bool isState) => isWorked = isState;
    }
}
