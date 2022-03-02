namespace EF_Repository_Pattern.Interface;

public interface IBasePropertyKey<Tkey>
{
    public Tkey Id { get; set; }
}
