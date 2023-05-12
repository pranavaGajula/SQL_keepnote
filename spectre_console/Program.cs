using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Spectre.Console;

namespace spectre_console
{
    class keepnote
    {
        public static void createNote(SqlConnection con)
        {
            SqlDataAdapter adp = new SqlDataAdapter("select * from keepNote", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "note");
            var row = ds.Tables["note"].NewRow();

            Console.WriteLine("Enter Title");
            row["title"] = Console.ReadLine();
            Console.WriteLine("Enter description");
            row["note_description"] = Console.ReadLine();
            DateTime note_date = new DateTime();
            try
            {
                Console.Write("Enter Date(DD/MM/YYYY): ");
                row["note_date"] = DateTime.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Enter in format dd/mm/yyyy");
                return;
            }
            ds.Tables[0].Rows.Add(row);
            SqlCommandBuilder builder = new SqlCommandBuilder(adp);
            adp.Update(ds, "note");
            Console.WriteLine("Row Added Successfully");
        }
        public static void view_all_Note(SqlConnection con)
        {
            SqlDataAdapter adp = new SqlDataAdapter("select * from keepNote", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "note");
            var row = new Table();
            row.AddColumn("id");
            row.AddColumn("title");
            row.AddColumn("note_description");
            row.AddColumn("note_date");


            for (int i = 0; i < ds.Tables["note"].Rows.Count; i++)
            {
                    row.AddRow(ds.Tables["note"].Rows[i][0].ToString(), ds.Tables["note"].Rows[i][1].ToString(), ds.Tables["note"].Rows[i][2].ToString(), ds.Tables["note"].Rows[i][3].ToString());
                
            }
            AnsiConsole.Write(row);



        }
        public static void view_note(SqlConnection con)
        {
            Console.WriteLine("enter ID");
            int nid = Convert.ToInt16(Console.ReadLine());
            
                SqlDataAdapter adp = new SqlDataAdapter($"select * from keepNote where id={nid}", con);
                DataSet ds = new DataSet();
                adp.Fill(ds, "note");
                var row = new Table();            
                row.AddColumn("id");
                row.AddColumn("title");
                row.AddColumn("note_description");
                row.AddColumn("note_date");
            try
            {
                for (int i = 0; i < ds.Tables["note"].Rows.Count; i++)
                {


                    row.AddRow(ds.Tables["note"].Rows[i][0].ToString(), ds.Tables["note"].Rows[i][1].ToString(), ds.Tables["note"].Rows[i][2].ToString(), ds.Tables["note"].Rows[i][3].ToString());
                }
                AnsiConsole.Write(row);
            }
            catch (Exception)
            {
                Console.WriteLine($"ID not exist with number{nid}");
                return;
            }
        }
        public static void delete_note(SqlConnection con)
        {
            Console.WriteLine("enter ID");

            int id = Convert.ToInt16(Console.ReadLine());
            SqlDataAdapter adp = new SqlDataAdapter($"Select * from keepNote where Id={id}", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "note");
            
            try
            { 
                ds.Tables["note"].Rows[0].Delete();
                
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                adp.Update(ds, "note");
                Console.WriteLine("Row deleted Successfully");
            }
            catch (Exception)
            {
                Console.WriteLine($"ID not exist with number{id}");
                return;
            }
            

        }
        public static void update_note(SqlConnection con)
        {
            Console.WriteLine("enter updated id");
            int nid = Convert.ToInt16(Console.ReadLine());

            SqlDataAdapter adp = new SqlDataAdapter($"Select * from keepNote where Id={nid}", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "note");
            try
            {
                Console.WriteLine("Enter Title");
                ds.Tables["note"].Rows[0][1] = Console.ReadLine();

                Console.WriteLine("Enter description");
                ds.Tables["note"].Rows[0][2] = Console.ReadLine();
                DateTime date = new DateTime();
                Console.Write("Enter Date(DD/MM/YYYY): ");
                ds.Tables["note"].Rows[0][3] = DateTime.Parse(Console.ReadLine());
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                adp.Update(ds, "note");
                Console.WriteLine("Database Updated");
            }
            catch
            {
                Console.WriteLine($"ID not exist with number{nid}");
            }

        }
    }
        internal class Program
        {
            static void Main(string[] args)
            {
                SqlConnection con = new SqlConnection("Server=IN-5YC79S3; database=TestDB; Integrated Security=true");
                string res;
                AnsiConsole.Write(new FigletText("Keep Note").Centered().Color(Color.Red));
                do
                {
                    var choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What's your [yellow]choice[/]")
                        .AddChoices(new[] {  "CreateNotes", "View Notes", "View All Notes", "Update Notes", "Delete Notes" }));
                    switch (choice)
                    {
                        case "CreateNotes":
                            {
                                keepnote.createNote(con);
                                break;
                            }
                        case "View Notes":
                            {
                                keepnote.view_note(con);

                                break;
                            }
                        case "View All Notes":
                            {
                                keepnote.view_all_Note(con);

                                break;
                            }
                        case "Update Notes":
                            {
                                keepnote.update_note(con);

                                break;
                            }
                        case "Delete Notes":
                            {
                                keepnote.delete_note(con);

                                break;
                            }
                    }
                   
                    //AnsiConsole.Write(Table);


                  
                   res = AnsiConsole.Ask<string>("Do you wish to [green] continue y/n? [/] ");
                } while (res.ToLower() == "y");
            }
        }
    
}
