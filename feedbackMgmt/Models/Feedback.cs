namespace feedbackMgmt.Models;

public class Feedback{
    
    public int Sid { get; set; }
    public string? Sname { get; set; }
    public string? Fdate { get; set; }
    public string? Module { get; set; }
    public string? Faculty { get; set; }
    public int Rating { get; set; }
    public int Pskill { get; set; }
    public string? Fcomment { get; set; }
    public Feedback(){}

    public Feedback(int id){
        this.Sid=id;
    }
}