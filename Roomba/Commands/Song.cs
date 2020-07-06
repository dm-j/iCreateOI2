using System.Collections.Immutable;
using System.Linq;

namespace iCreateOI2.Commands
{
    public enum Note
    { 
        REST = 200,
        G0 = 31, GS0, 
        A1, AS1, B1, C1, CS1, D1, DS1, E1, F1, FS1, G1, GS1,
        A2, AS2, B2, C2, CS2, D2, DS2, E2, F2, FS2, G2, GS2,
        A3, AS3, B3, C3, CS3, D3, DS3, E3, F3, FS3, G3, GS3,
        A4, AS4, B4, C4, CS4, D4, DS4, E4, F4, FS4, G4, GS4,
        A5, AS5, B5, C5, CS5, D5, DS5, E5, F5, FS5, G5, GS5,
        A6, AS6, B6, C6, CS6, D6, DS6, E6, F6, FS6, G6, GS6,
        A7, AS7, B7
    }

    public class SongNumber : IDataBytes
    {
        public ImmutableArray<byte> Data { get; private set; }

        private SongNumber(int number)
        {
            Data = new[] { (byte)number }.ToImmutableArray();
        }

        public static SongNumber Song1 { get; } = new SongNumber(0);
        public static SongNumber Song2 { get; } = new SongNumber(1);
        public static SongNumber Song3 { get; } = new SongNumber(2);
        public static SongNumber Song4 { get; } = new SongNumber(3);
        public static SongNumber Immediate { get; } = new SongNumber(4);

    }

    public class Song : IDataBytes
    { 
        public ImmutableArray<byte> Data { get; private set; }
        public readonly SongNumber Number;

        private Song(SongNumber number, Melody melody)
        {
            Data = number.Data.Concat(melody.Data).ToImmutableArray();
            Number = number;
        }

        public static Song Define(SongNumber number, Melody melody) =>
            new Song(number, melody);
    }

    public class Melody : IDataBytes
    {
        public ImmutableArray<byte> Data { get; private set; }

        private Melody(params (Note note, int length64ths)[] notes)
        {
            Data = new[] { (byte)notes.Length }.Concat(notes.SelectMany(note => new[] { (byte)note.note, (byte)note.length64ths })).ToImmutableArray();
        }

        public static Melody Define(params (Note note, int length64ths)[] notes) =>
            new Melody(notes);
    }
}
