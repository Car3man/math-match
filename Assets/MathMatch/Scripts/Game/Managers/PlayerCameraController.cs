using MathMatch.Game.Behaviours;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.Managers
{
    public class PlayerCameraController : ITickable
    {
        private readonly Camera _sceneCamera;
        private readonly LevelGenerator _levelGenerator;

        private Player _playerToFollow;

        private const float MoveSpeed = 2f;

        public PlayerCameraController(
            Camera sceneCamera,
            LevelGenerator levelGenerator
            )
        {
            _sceneCamera = sceneCamera;
            _levelGenerator = levelGenerator;
        }

        public void Tick()
        {
            if (_playerToFollow != null)
            {
                MoveToPlayerSlightly();
            }
        }

        public void StartFollowPlayer(Player player)
        {
            if (_playerToFollow)
            {
                Debug.LogWarning("Already following the player.");
                return;
            }
            
            _playerToFollow = player;
        }

        public void StopFollowPlayer()
        {
            if (!_playerToFollow)
            {
                Debug.LogWarning("Didn't started follow the player.");
                return;
            }

            _playerToFollow = null;
        }
        
        public Vector3 GetProjectedPositionByCameraRight(Vector3 position)
        {
            var sceneCameraTrans = _sceneCamera.transform;
            var sceneCameraRight = sceneCameraTrans.right;
            sceneCameraRight.y = 0f;
            return Vector3.Project(position, sceneCameraRight);
        }

        public void MoveToPlayerForced()
        {
            var sceneCameraTrans = _sceneCamera.transform;
            var targetCameraPos = GetTargetCameraPos();
            sceneCameraTrans.position = targetCameraPos;
        }

        private void MoveToPlayerSlightly()
        {
            var sceneCameraTrans = _sceneCamera.transform;
            var sceneCameraPos = sceneCameraTrans.position;
            var targetCameraPos = GetTargetCameraPos();
            sceneCameraTrans.position = Vector3.MoveTowards(sceneCameraPos, targetCameraPos, Time.deltaTime * MoveSpeed);
        }

        private Vector3 GetTargetCameraPos()
        {
            var playerTrans = _playerToFollow.transform;
            var playerPos = playerTrans.position;
            
            var playerSegment = _levelGenerator.GetSegmentByPlace(_playerToFollow.Place);
            var playerSegmentCenter = (playerSegment.Start.transform.position + playerSegment.End.transform.position) * 0.5f;
            
            var sceneCameraTrans = _sceneCamera.transform;
            var sceneCameraPos = sceneCameraTrans.position;

            var targetPosByPlayer = new Vector3(
                playerPos.x + 2f,
                sceneCameraPos.y,
                playerPos.z - 2f
            );
            var playerPosXSegmentCenterXDiff = (GetProjectedPositionByCameraRight(playerPos) -
                                                GetProjectedPositionByCameraRight(playerSegmentCenter)).x;
            var correctTargetPos = targetPosByPlayer - new Vector3(playerPosXSegmentCenterXDiff, 0f, playerPosXSegmentCenterXDiff);
            return correctTargetPos;
        }
    }
}
