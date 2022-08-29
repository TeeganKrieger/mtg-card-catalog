using Microsoft.AspNetCore.Http.Extensions;
using MTGCC.Extensions;
using System.Text;

namespace MTGCC.Translators
{
    public class SearchBody
    {
        public string? NameQuery { get; set; } = null;
        public string? DescriptionQuery { get; set; } = null;
        public TypeQuery[]? TypeQueries { get; set; } = null;
        public bool UsePartialTypeMatching { get; set; }
        public string? ColorQuery { get; set; } = null;
        public ColorConstraint ColorConstraint { get; set; }
        public UniqueStrategy UniqueStrategy { get; set; }
        public SortMode SortMode { get; set; }
        public bool SortAscending { get; set; }
        public int Page { get; set; }


        public SearchBody()
        {

        }

        public override string ToString()
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            StringBuilder qBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.NameQuery))
                qBuilder.AppendFormat("{0} ", this.NameQuery);

            if (!string.IsNullOrWhiteSpace(this.DescriptionQuery))
            {
                string[] splitQuery = DescriptionQuery.Split(' ');
                if (splitQuery.Length > 1) 
                {
                    qBuilder.Append("(");
                    splitQuery.ForEach(x => qBuilder.AppendFormat("oracle:{0} ", x));
                    qBuilder.Append(") ");
                }
                else
                {
                    qBuilder.AppendFormat("oracle:{0} ", splitQuery[0]);
                }
            }

            if (this.TypeQueries != null && this.TypeQueries.Length > 0)
            {
                string splitter = this.UsePartialTypeMatching ? " OR " : " ";

                if (this.TypeQueries.Length > 1)
                {
                    qBuilder.Append("(");
                    this.TypeQueries.ForEach(x => qBuilder.AppendFormat("type:{0}{1}", x, splitter));
                    qBuilder.Append(") ");
                }
                else
                {
                    qBuilder.AppendFormat("type:{0} ", this.TypeQueries[0]);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(this.ColorQuery))
            {
                switch (this.ColorConstraint)
                {
                    case ColorConstraint.Exactly:
                        qBuilder.AppendFormat("color={0} ", ColorQuery);
                        break;
                    case ColorConstraint.Including:
                        qBuilder.AppendFormat("color>={0} ", ColorQuery);
                        break;
                    case ColorConstraint.AtMost:
                        qBuilder.AppendFormat("color<={0} ", ColorQuery);
                        break;
                }
            }

            string uniqueString = "";
            
            switch (this.UniqueStrategy)
            {
                case UniqueStrategy.Cards:
                    uniqueString = "cards";
                    break;
                case UniqueStrategy.Art:
                    uniqueString = "art";
                    break;
                case UniqueStrategy.Prints:
                    uniqueString = "prints";
                    break;
            }

            string orderString = "";

            switch (this.SortMode)
            {
                case SortMode.Name:
                    orderString = "name";
                    break;
                case SortMode.Rarity:
                    orderString = "rarity";
                    break;
                case SortMode.Color:
                    orderString = "color";
                    break;
                case SortMode.ConvertedManaCost:
                    orderString = "cmc";
                    break;
                case SortMode.Power:
                    orderString = "power";
                    break;
                case SortMode.Toughness:
                    orderString = "toughness";
                    break;
            }

            string directionString = this.SortAscending ? "asc" : "desc";

            string qString = qBuilder.ToString();

            if (qString.Length == 0)
                return null;

            queryBuilder.Add("q", qString);
            queryBuilder.Add("unique", uniqueString);
            queryBuilder.Add("order", orderString);
            queryBuilder.Add("dir", directionString);
            queryBuilder.Add("include_extras", "false");
            queryBuilder.Add("include_multilingual", "false");
            queryBuilder.Add("include_variations", "false");
            queryBuilder.Add("page", this.Page.ToString());
            queryBuilder.Add("format", "json");

            return queryBuilder.ToQueryString().Value;
        }
    }
}
