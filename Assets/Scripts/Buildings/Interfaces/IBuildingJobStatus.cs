using System.Diagnostics;
using DebugCustomSystem;

namespace Building.Additional
{
    public interface IBuildingJobStatus
    {
        bool isWorked { get; protected set; }

        bool isBuyed { get; }


        void ChangeJobStatus(in bool isState)
        {
            if (isBuyed == false)
                return;

            isWorked = isState;
            DebugSystem.Log($"IsWorked status: {isWorked}", DebugSystem.SelectedColor.Blue, tag: "Building");
        }
    }
}
