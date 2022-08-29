namespace MTGCC.Translators
{
    public struct TypeQuery
    {
        public bool Include { get; set; }
        public string Query { get; set; }

        public override string ToString()
        {
            if (this.Include)
                return $"type:{Query}";
            else
                return $"-type:{Query}";
        }

    }
}
