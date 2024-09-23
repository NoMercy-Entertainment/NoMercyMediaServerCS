using NoMercy.NmSystem;
using Serilog.Events;

namespace NoMercy.Encoder.Core;

public class SupImageExtractor
{
    public static void ExtractImages(string supFilePath, string outputDirectory)
    {
        using FileStream fs = new FileStream(supFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(fs);
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            // Read the PGS packet header
            byte[] header = reader.ReadBytes(13);
            if (header.Length < 13)
                break;

            // Check for PGS start code (0x5047)
            if (header[0] == 0x50 && header[1] == 0x47)
            {
                int packetLength = (header[2] << 8) | header[3];
                byte[] packetData = reader.ReadBytes(packetLength);
                
                // Logger.Encoder($"Processing PGS packet of length {packetLength}", LogEventLevel.Verbose);

                // Process the PGS packet
                ProcessPgsPacket(packetData, outputDirectory);
            }
        }
    }

    private static void ProcessPgsPacket(byte[] packetData, string outputDirectory)
    {
        int index = 0;
        while (index < packetData.Length)
        {
            if (index + 3 > packetData.Length)
                break;

            int segmentType = packetData[index];
            int segmentLength = (packetData[index + 1] << 8) | packetData[index + 2];

            Logger.Encoder($"Processing segment type {segmentType} of length {segmentLength}", LogEventLevel.Verbose);

            if (index + 3 + segmentLength > packetData.Length)
                break;

            byte[] segmentData = new byte[segmentLength];
            Array.Copy(packetData, index + 3, segmentData, 0, segmentLength);

            if (segmentLength > 30) // ODS segment
            {
                Logger.Encoder("ODS segment found", LogEventLevel.Verbose);
                ExtractImageFromOds(segmentData, outputDirectory);
            }

            index += 3 + segmentLength;
        }
    }

    private static void ExtractImageFromOds(byte[] segmentData, string outputDirectory)
    {
        if (segmentData.Length < 6)
        {
            Logger.Encoder("Segment data too short for ODS", LogEventLevel.Warning);
            return;
        }

        int objectId = (segmentData[0] << 8) | segmentData[1];
        int objectVersion = segmentData[2];
        int objectSequence = segmentData[3];
        int objectDataLength = (segmentData[4] << 8) | segmentData[5];

        Logger.Encoder($"Extracting ODS: objectId={objectId}, objectVersion={objectVersion}, objectSequence={objectSequence}, objectDataLength={objectDataLength}", LogEventLevel.Verbose);

        if (segmentData.Length < 6 + objectDataLength)
        {
            Logger.Encoder("Segment data length mismatch", LogEventLevel.Warning);
            return;
        }

        byte[] imageData = new byte[objectDataLength];
        Array.Copy(segmentData, 6, imageData, 0, objectDataLength);

        // Save the image data to a file
        string outputFilePath = Path.Combine(outputDirectory, $"image_{objectId}_{objectVersion}_{objectSequence}.bin");
        Logger.Encoder($"Saving image to {outputFilePath}", LogEventLevel.Verbose);
        File.WriteAllBytes(outputFilePath, imageData);
    }
}