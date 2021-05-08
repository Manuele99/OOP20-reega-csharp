namespace Reega.Pola
{
    public interface IReegaExporter
    {
        /// <summary>
        /// Export data to the given file instance
        /// </summary>
        /// <param name="file">to write the output on</param>
        void Export(string file);
    }
}
