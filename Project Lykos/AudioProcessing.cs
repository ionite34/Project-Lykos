using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Project_Lykos;

public class AudioProcessing
{

    //  Use the NAudio to convert 
    public static void Resample(string inFile, string outFile, int sampleRate, int channels)
    {
        // Check if the subfolder the outFile is in exists, if not create it
        if (!Directory.Exists(Path.GetDirectoryName(outFile)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outFile)!);
        }
        var outFormat = new WaveFormat(sampleRate, channels);
        using var reader = new WaveFileReader(inFile);
        using var resampler = new MediaFoundationResampler(reader, outFormat);
        resampler.ResamplerQuality = 60;
        WaveFileWriter.CreateWaveFile(outFile, resampler);
    }
}