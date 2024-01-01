using App.Store.Model;

using Microsoft.Data.Sqlite;

namespace App.Store
{
    public class ChangeDB
    {
        private Command _command;
        public ChangeDB()
        {
            _command = new Command();
        }
        public List<WordData> GetWords()
        {
            List<WordData> temp = new List<WordData>();
            SqliteDataReader _dataTemp = _command.InitialTable("Select * from Word");

            if (_dataTemp.HasRows)
            {
                while (_dataTemp.Read())
                {
                    temp.Add(new WordData()
                    {
                        //ID = int.Parse(_dataTemp.GetValue(0).ToString()),
                        Full = _dataTemp.GetValue(1).ToString(),
                        Prefix = _dataTemp.GetValue(2).ToString(),
                        Root = _dataTemp.GetValue(3).ToString(),
                        Suffix = _dataTemp.GetValue(4).ToString()
                    });
                }
            }

            return temp;
        }
        public void Add(WordData word)
        {
            _command.Execute($"Insert into Word (Full, Root, Prefix, Suffix) values ('{word.Full}', '{word.Root}', '{word.Prefix}', '{word.Suffix}')");
        }
    }
}