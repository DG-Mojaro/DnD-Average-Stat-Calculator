class Dice
{
    public Dice(int noOfFaces)
    {
        this.NumberOfFaces = noOfFaces;
        this.Faces = new int[noOfFaces];
        for (int i = 0; i < this.Faces.Length; i++)
        {
            this.Faces[i] = Convert.ToInt32(i) + 1;
        }
    }

    public int NumberOfFaces 
    { get; private set; }

    public int[] Faces
    { get; private set; }

    public int Roll()
    {
        int roll = new Random().Next(1, (this.NumberOfFaces + 1));
        return roll;
    }

}