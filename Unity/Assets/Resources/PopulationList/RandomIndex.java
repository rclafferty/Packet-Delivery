import java.io.*;
import java.util.*;

public class RandomIndex
{
    public static void main(String[] args) throws IOException
    {
        // System.out.println("Arguments: ");
        // for (int i = 0; i < args.length; i++)
        // {
        //     System.out.println("args[" + i + "] : " + args[i]);
        // }

        // System.out.println("Reading from: " + args[0]);
        // System.out.println("Writing to:   " + args[0] + "\b\b\b\b_with_index.txt");

        // Scanner s = new Scanner(System.in);
        Scanner inFile = new Scanner(new File(args[0]));
        int maxNumber = Integer.parseInt(args[1]);

        int numberOfNames = Integer.parseInt(inFile.nextLine());
        // pw.println(numberOfNames);
        Random r = new Random();
        System.out.println(numberOfNames);

        for (int i = 0; i < numberOfNames; i++)
        {
            String name = inFile.nextLine();
            // pw.println(name + "\t" + r.nextInt());
            System.out.println(name + "\t" + r.nextInt(maxNumber));
        }

        // pw.close();
        inFile.close();
        // s.close();
    }
}