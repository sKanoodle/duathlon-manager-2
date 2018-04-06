using DuathlonManager2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuathlonManager2.ViewModel
{
    public class GoogleImportViewModel : ViewModelBase
    {
        public ObservableCollection<(string Error, string Row)> Errors { get; } = new ObservableCollection<(string Error, string Row)>();
        private readonly Dictionary<string, int> ImportOrder;
        private readonly List<Starter> Starters = new List<Starter>();


        private int _SmallKidsUpperAgeBoundary = 10;
        /// <summary>
        /// upper inclusive age boundary for small children
        /// </summary>
        public int SmallKidsUpperAgeBoundary
        {
            get => _SmallKidsUpperAgeBoundary;
            set => SetValue(ref _SmallKidsUpperAgeBoundary, value);
        }

        private int _MediumKidsUpperAgeBoundary = 12;
        /// <summary>
        /// upper inclusive age boundary for medium children
        /// </summary>
        public int MediumKidsUpperAgeBoundary
        {
            get => _MediumKidsUpperAgeBoundary;
            set => SetValue(ref _MediumKidsUpperAgeBoundary, value);
        }

        private int _LargeKidsUpperAgeBoundary = 13;
        /// <summary>
        /// upper inclusive age boundary for large children
        /// </summary>
        public int LargeKidsUpperAgeBoundary
        {
            get => _LargeKidsUpperAgeBoundary;
            set => SetValue(ref _LargeKidsUpperAgeBoundary, value);
        }

        public GoogleImportViewModel()
        {

        }

        private void ParseStarter(string[] row)
        {
            //rowdump for error messages
            string rowDump = String.Join(", ", row);

            //parsing teamname
            string teamName = row[ImportOrder[nameof(Starter.TeamName)]];

            //parsing year of birth
            if (!int.TryParse(row[ImportOrder[nameof(Person.YearOfBirth)]], out int yearOfBirth))
            {
                Errors.Add(("cannot parse year of birth", rowDump));
                return;
            }

            //parsing sex
            Sex sex;
            string inputSex = row[ImportOrder[nameof(Person.Sex)]].ToLower();
            if (inputSex == "männlich")
                sex = Sex.Male;
            else if (inputSex == "weiblich")
                sex = Sex.Female;
            else
            {
                Errors.Add(("cannot parse sex", rowDump));
                return;
            }

            //assigning all the data to a new person
            Person parsedPerson = new Person
            {
                FirstName = row[ImportOrder[nameof(Person.FirstName)]],
                LastName = row[ImportOrder[nameof(Person.LastName)]],
                Club = row[ImportOrder[nameof(Person.Club)]],
                Email = row[ImportOrder[nameof(Person.FirstName)]],
                YearOfBirth = yearOfBirth,
                Sex = sex,
            };

            //competition from spreadsheet
            string inputCompetition = row[ImportOrder[nameof(Starter.Competition)]];

            //case of single starter
            if (String.IsNullOrEmpty(teamName))
            {
                Competition competition = ParseSinglesCompetition(parsedPerson, inputCompetition);
                if (competition == Competition.None)
                {
                    Errors.Add(("failed to parse single starter competition", rowDump));
                    return;
                }
                Starter singleStarter = new Starter
                {
                    Competition = competition,
                    StartNumber = NextStartNumber,
                    Persons = { parsedPerson },
                };
                Starters.Add(singleStarter);
                return;
            }

            Starter relayStarter = Starters.SingleOrDefault(s => s.TeamName == teamName);

            //case of second relay starter
            if (relayStarter != default(Starter))
            {
                Competition competition = ParseRelayCompetition(parsedPerson, relayStarter.Persons.First(), inputCompetition);
                if (competition == Competition.None)
                {
                    Errors.Add(("failed to parse relay starter competition", rowDump));
                    return;
                }
                relayStarter.Competition = competition;

                if (inputCompetition.Contains("Schwimmer"))
                    relayStarter.Persons.Insert(0, parsedPerson);
                else if (inputCompetition.Contains("Läufer"))
                    relayStarter.Persons.Add(parsedPerson);
                else
                {
                    Errors.Add(("cannot determine whether person is runner or swimmer", rowDump));
                    return;
                }
            }

            //case of first relay starter
            relayStarter = new Starter
            {
                TeamName = teamName,
                StartNumber = NextRelayStartNumber,
                Persons = { parsedPerson },
            };
            Starters.Add(relayStarter);
        }

        private Competition ParseSinglesCompetition(Person person, string competition)
        {
            if (competition == "Hauptwettkampf (Einzel)")
            {
                if (person.Sex == Sex.Male)
                    return Competition.MainSingleMale;
                if (person.Sex == Sex.Female)
                    return Competition.MainSingleFemale;
            }
            if (competition == "Jedermann-Wettkampf (Einzel)")
            {
                if (person.Sex == Sex.Male)
                    return Competition.SubSingleMale;
                if (person.Sex == Sex.Female)
                    return Competition.SubSingleFemale;
            }
            if (competition == "Kinderwettkampf")
            {
                if (person.Sex == Sex.Male)
                {
                    if (person.YearOfBirth >= SmallKidsLowerBoundary)
                        return Competition.ChildSmallMale;
                    if (person.YearOfBirth >= MediumKidsLowerBoundary)
                        return Competition.ChildMediumMale;
                    if (person.YearOfBirth >= LargeKidsLowerBoundary)
                        return Competition.ChildLargeMale;
                }
                if (person.Sex == Sex.Female)
                {
                    if (person.YearOfBirth >= SmallKidsLowerBoundary)
                        return Competition.ChildSmallFemale;
                    if (person.YearOfBirth >= MediumKidsLowerBoundary)
                        return Competition.ChildMediumFemale;
                    if (person.YearOfBirth >= LargeKidsLowerBoundary)
                        return Competition.ChildLargeFemale;
                }
            }

            return Competition.None;

            //if (competition == "Hauptwettkampf (Einzel)" && person.Sex == Sex.Male)
            //    return Competition.MainSingleMale;
            //if (competition == "Hauptwettkampf (Einzel)" && person.Sex == Sex.Female)
            //    return Competition.MainSingleFemale;
            //if (competition == "Jedermann-Wettkampf (Einzel)" && person.Sex == Sex.Male)
            //    return Competition.SubSingleMale;
            //if (competition == "Jedermann-Wettkampf (Einzel)" && person.Sex == Sex.Female)
            //    return Competition.SubSingleFemale;
            //if (competition == "Kinderwettkampf" && optionalAge < 11 && isMale)
            //    return Competition.ChildSmallMale;
            //if (competition == "Kinderwettkampf" && optionalAge < 11 && isMale == false)
            //    return Competition.ChildSmallFemale;
            //if (competition == "Kinderwettkampf" && optionalAge < 13 && isMale)
            //    return Competition.ChildMediumMale;
            //if (competition == "Kinderwettkampf" && optionalAge < 13 && isMale == false)
            //    return Competition.ChildMediumFemale;
            //if (competition == "Kinderwettkampf" && optionalAge < 14 && isMale)
            //    return Competition.ChildLargeMale;
            //if (competition == "Kinderwettkampf" && optionalAge < 14 && isMale == false)
            //    return Competition.ChildLargeFemale;
        }

        private Competition ParseRelayCompetition(Person person1, Person person2, string competition)
        {
            if (competition == "Hauptwettkampf (Staffel-Schwimmer)" || competition == "Hauptwettkampf (Staffel-Läufer)")
            {
                if (person1.Sex == Sex.Male && person2.Sex == Sex.Male)
                    return Competition.MainRelayMale;
                if (person1.Sex == Sex.Female && person2.Sex == Sex.Female)
                    return Competition.MainRelayMale;
                if (person1.Sex == Sex.Male && person2.Sex == Sex.Female || person1.Sex == Sex.Female && person2.Sex == Sex.Male)
                    return Competition.MainRelayMixed;
            }
            if (competition == "Jedermann-Wettkampf (Staffel-Schwimmer)" || competition == "Jedermann-Wettkampf (Staffel-Läufer)")
                return Competition.SubRelay;
            return Competition.None;

            //if ((competition == "Hauptwettkampf (Staffel-Schwimmer)" || competition == "Hauptwettkampf (Staffel-Läufer)") && isMale && optionalRelayIsMale)
            //    return Competition.MainRelayMale;
            //if ((competition == "Hauptwettkampf (Staffel-Schwimmer)" || competition == "Hauptwettkampf (Staffel-Läufer)") && isMale == false && optionalRelayIsMale == false)
            //    return Competition.MainRelayFemale;
            //if ((competition == "Hauptwettkampf (Staffel-Schwimmer)" || competition == "Hauptwettkampf (Staffel-Läufer)") && (isMale && optionalRelayIsMale == false || isMale == false && optionalRelayIsMale))
            //    return Competition.MainRelayMixed;
            //if (competition == "Jedermann-Wettkampf (Staffel-Schwimmer)" || competition == "Jedermann-Wettkampf (Staffel-Läufer)")
            //    return Competition.SubRelay;
        }
    }
}
