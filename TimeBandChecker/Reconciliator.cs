using System.Collections.Generic;

namespace TimeBandChecker
{
    public class Reconciliator
    {
        private readonly List<string> _adamsData;
        private readonly List<string> _ipsosData;
        private List<string> finalList;
        public Reconciliator(List<string> adamsData, List<string> ipsosData)
        {
            _adamsData = adamsData;
            _ipsosData = ipsosData;
            finalList = new List<string>();
        }

        public List<string> Reconcile()
        {

            return finalList;
        }
    }
}
