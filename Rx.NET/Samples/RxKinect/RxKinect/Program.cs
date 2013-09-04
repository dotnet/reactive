using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Kinect;

namespace RxKinect
{
    class Program
    {
        static void Main()
        {
            var kinect = KinectSensor.KinectSensors.Single();

            Console.WriteLine(kinect.DeviceConnectionId);

            var skeletonFrames = Observable.FromEventPattern<SkeletonFrameReadyEventArgs>(
                addHandler: h => kinect.SkeletonFrameReady += h,
                removeHandler: h => kinect.SkeletonFrameReady -= h
            );

            var skeletons = skeletonFrames
                .Select(sf =>
                {
                    using (var frame = sf.EventArgs.OpenSkeletonFrame())
                    {
                        var sd = new Skeleton[frame.SkeletonArrayLength];
                        frame.CopySkeletonDataTo(sd);
                        return sd;
                    }
                });

            var joints = from sd in skeletons
                         let tracked = sd.SingleOrDefault(s => s.TrackingState == SkeletonTrackingState.Tracked)
                         where tracked != null
                         select tracked.Joints;

            var left = from joint in joints
                       let pos = joint[JointType.HandLeft].Position
                       select pos;

            var rel = (from pos in left
                       where Math.Abs(pos.X) > 0.2
                       select pos.X < 0 ? "left" : "right")
                      .DistinctUntilChanged();

            var res = from moves in rel.Buffer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1))
                      where moves.Count >= 3
                      select true;

            res.Subscribe(_ =>
            {
                Console.WriteLine("Swinging left hand");
            });

            kinect.SkeletonStream.Enable();
            kinect.Start();

            Console.ReadLine();
        }
    }
}
