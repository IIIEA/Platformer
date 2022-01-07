using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PatrolPath
{
    public class Mover
    {
        private PatrolPath _path;
        private float _interpolantValue = 0;
        private float _duration;
        private float _startTime;

        public Vector2 Position
        {
            get
            {
                _interpolantValue = Mathf.InverseLerp(0, _duration, Mathf.PingPong(Time.time - _startTime, _duration));
                return _path.transform.TransformPoint(Vector2.Lerp(_path.startPosition, _path.endPosition, _interpolantValue));
            }
        }

        public Mover(PatrolPath path, float speed)
        {
            _path = path;
            _duration = (path.endPosition - path.startPosition).magnitude / speed;
            _startTime = Time.time;
        }

    }
}