﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeBandChecker
{
    internal class Reconciliator
    {
        private readonly List<string> _adamsData;
        private readonly List<string> _ipsosData;
        private List<string> finalList;
        private List<Adams> adamsRecords;
        private List<Ipsos> ipsosRecords;
        private List<Reconciled> reconciledRecords;
        public Reconciliator(List<string> adamsData, List<string> ipsosData)
        {
            _adamsData = adamsData;
            _ipsosData = ipsosData;
            finalList = new List<string>();
            adamsRecords = new List<Adams>();
            ipsosRecords = new List<Ipsos>();
            GenerateRecords();
        }

        public List<Reconciled> Reconcile()
        {
            foreach (var adams in adamsRecords)
            {
                if (adams.HasTimeband)
                {
                    ProcessTimebands(adams);
                }
                else
                {
                    ProcessNoneTimebands(adams);
                }
            }

            if (adamsRecords.Count > 0)
                ProcessLastRecords();

            return reconciledRecords;
        }

        private void ProcessLastRecords()
        {
            foreach (var adams in adamsRecords)
            {
                foreach (var ipsos in ipsosRecords)
                {
                    if (IsROS(adams.TimebandStart, adams.TimebandEnd, ipsos.IpsosTime))
                    {
                        reconciledRecords.Add(
                            new Reconciled()
                            {
                                AdamsRow = adams.Row,
                                IpsosRow = ipsos.Row,
                                Status = "ROS"
                            }
                            );
                        adamsRecords.Remove(adams);
                        ipsosRecords.Remove(ipsos);
                        break;
                    }
                }
            }

            if(adamsRecords.Count > 0)
            {
                foreach (var adams in adamsRecords)
                {
                    reconciledRecords.Add(
                        new Reconciled()
                        {
                            AdamsRow = adams.Row,
                            Status = "FALSE"
                        }
                        );
                }
            }

            if (ipsosRecords.Count > 0)
            {
                foreach (var ipsos in ipsosRecords)
                {
                    reconciledRecords.Add(
                        new Reconciled()
                        {
                            IpsosRow = ipsos.Row,
                            Status = "FALSE"
                        }
                        );
                }
            }
        }

        private void ProcessNoneTimebands(Adams adams)
        {
            var match = ipsosRecords.Where(x => x.SubBrand == adams.MappedSubBrand).FirstOrDefault();
            if (match != null)
            {
                reconciledRecords.Add(
                        new Reconciled()
                        {
                            AdamsRow = adams.Row,
                            IpsosRow = match.Row,
                            Status = "TRUE"
                        }
                        );
                ipsosRecords.Remove(match);
                adamsRecords.Remove(adams); 
            }

        }

        private void ProcessTimebands(Adams adams)
        {
            var matches = ipsosRecords.Where(x => x.SubBrand == adams.MappedSubBrand).ToList();
            foreach (var match in matches)
            {
                if (IsWithinTimebandRange(adams.TimebandStart, adams.TimebandEnd, match.IpsosTime))
                {
                    reconciledRecords.Add(
                        new Reconciled()
                        {
                            AdamsRow = adams.Row,
                            IpsosRow = match.Row,
                            Status = "TRUE"
                        }
                        );
                    ipsosRecords.Remove(match);
                    adamsRecords.Remove(adams);
                    break;
                }
            }
        }

        private bool IsWithinTimebandRange(string timebandStart, string timebandEnd, string ipsosTime)
        {
            if (string.IsNullOrWhiteSpace(timebandStart) && string.IsNullOrWhiteSpace(timebandEnd))
                return false;

            if (!string.IsNullOrWhiteSpace(timebandStart) && string.IsNullOrWhiteSpace(timebandEnd))
                timebandEnd = timebandStart;

            bool isValidTime = DateTime.TryParse(TimeFormatter(timebandStart), out DateTime tbStart);
            if (!isValidTime)
                return false;

            isValidTime = DateTime.TryParse(TimeFormatter(timebandEnd), out DateTime tbEnd);
            if (!isValidTime)
                return false;

            isValidTime = DateTime.TryParse(ipsosTime, out DateTime ipsos);
            if (!isValidTime)
                return false;

            if (tbStart == tbEnd)
                tbEnd = tbEnd.Add(new TimeSpan(1, 0, 0));

            return ipsos.TimeOfDay >= tbStart.TimeOfDay && ipsos.TimeOfDay <= tbEnd.TimeOfDay;
        }

        private string TimeFormatter(string time)
        {
            time = time.Trim();
            if (time.Contains('.'))
                time = time.Replace('.', ':');
            if (!time.Contains(":"))
                time = time.Substring(0, time.Length - 2).Trim() + ":00" + time.Substring(time.Length - 2);

            return time;
        }

        private bool IsROS(string timebandStart, string timebandEnd, string ipsosTime)
        {
            if (string.IsNullOrWhiteSpace(timebandStart) || string.IsNullOrWhiteSpace(timebandEnd))
                return false;

            bool isValidTime = DateTime.TryParse(TimeFormatter(timebandStart), out DateTime tbStart);
            if (!isValidTime)
                return false;

            isValidTime = DateTime.TryParse(TimeFormatter(timebandEnd), out DateTime tbEnd);
            if (!isValidTime)
                return false;

            isValidTime = DateTime.TryParse(ipsosTime, out DateTime ipsos);
            if (!isValidTime)
                return false;

            var overshot = ipsos - tbEnd;
            var undershot = tbStart - ipsos;
            TimeSpan threshold = new TimeSpan(0, 30, 0);
            bool isWithinRange = ipsos.TimeOfDay >= tbStart.TimeOfDay && ipsos.TimeOfDay <= tbEnd.TimeOfDay;

            return (overshot.Duration() <= threshold && !isWithinRange) || (undershot.Duration() <= threshold && !isWithinRange);
        }
        private void GenerateRecords()
        {
            foreach (var data in _adamsData)
            {
                var tempModel = data.Split('|');
                adamsRecords.Add(
                    new Adams()
                    {
                        Row = tempModel[0],
                        VariantName = tempModel[1],
                        Material = tempModel[2],
                        TimebandStart = tempModel[3],
                        TimebandEnd = tempModel[4],
                        HasTimeband = bool.Parse(tempModel[5]),
                        MappedSubBrand = tempModel[6]
                    }
                    );
            }

            foreach (var data in _ipsosData)
            {
                var tempModel = data.Split('|');
                ipsosRecords.Add(
                    new Ipsos()
                    {
                        Row = tempModel[0],
                        IpsosTime = tempModel[1],
                        SubBrand = tempModel[2]
                    }
                    );
            }
        }


    }
}
