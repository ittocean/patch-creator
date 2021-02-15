namespace PatchCreator2
{
	public class FilePath
	{
		public string Path { get; set; }

		public override bool Equals(object i_Other)
		{
			FilePath other = i_Other as FilePath;
			return (other != null) ? Path.Equals(other.Path) : false;
		}

		public override int GetHashCode()
		{
			return Path.GetHashCode();
		}

		public override string ToString()
		{
			return Path;
		}
	}
}
