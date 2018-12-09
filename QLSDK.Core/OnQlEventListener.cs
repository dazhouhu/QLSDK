namespace QLSDK.Core
{
    public interface OnQlEventListener
    {
        void DoEvent(QLEvent evt);
    }
}