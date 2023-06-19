using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Screen
{
    public class ScreenClick2World : MonoBehaviour
    {
        public Transform m_TF_CurrentTarget { get => m_tf_currentTarget; }
        public Transform[] m_TF_CurrentTargets { get => m_tf_currentTargets; }

        [Header("���� ������ �ʿ��� �ɼǵ�")]
        [Tooltip("�浹 ��� ���̾�")]
        [SerializeField] private LayerMask m_targetLayer;

        [Header("Ȯ�ο� �ɼǵ�")]
        [SerializeField] private RectTransform m_rtf_point;
        [SerializeField] private bool m_activeOnStart;

        [SerializeField] private Transform m_tf_currentTarget;
        [SerializeField] private Transform[] m_tf_currentTargets;
        private Ray ray;
        private RaycastHit hit;
        private RaycastHit[] hits;

        public void Click(bool checkMultiple = false)
        {
            ClickLogic(checkMultiple);
        }
        public bool TryClick(out Transform outResult)
        {
            ClickLogic(false);

            outResult = null;
            if (m_tf_currentTarget != null && 
                m_tf_currentTarget != null)
            {
                outResult = m_tf_currentTarget;
                return true;
            }
            return false;
        }
        public bool TryClick(out Transform[] outResult)
        {
            ClickLogic(true);

            outResult = null;
            if (m_tf_currentTarget != null &&
                m_tf_currentTarget.TryGetComponent(out outResult))
            {
                outResult = m_tf_currentTargets;
                return true;
            }
            return false;
        }
        public bool TryClick<T>(out T outResult) where T : Component
        {
            Click();
            outResult = null;
            if (m_tf_currentTarget != null &&
                m_tf_currentTarget.TryGetComponent(out outResult))
            {
                return true;
            }
            return false;
        }
        public bool TryClick<T>(out T[] outResult) where T : Component
        {
            Click();
            outResult = null;

            if (m_tf_currentTarget != null)
            {
                List<T> list = new List<T>();
                T temp;
                for (int i = 0; i < m_tf_currentTargets.Length; i++)
                {
                    if (m_tf_currentTargets[i].TryGetComponent(out temp))
                    {
                        list.Add(temp);
                    }
                }

                if (list.Count == 0) return false;

                outResult = new T[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    outResult[i] = list[i];
                }
                return true;
            }

            return false;
        }

        public void ClickByMouse(bool checkMultiple = false)
        {
            m_rtf_point.position = Input.mousePosition;
            Click(checkMultiple);
        }
        public bool TryClickByMouse(out Transform outResult)
        {
            m_rtf_point.position = Input.mousePosition;
            return TryClick(out outResult);
        }
        public bool TryClickByMouse(out Transform[] outResult)
        {
            m_rtf_point.position = Input.mousePosition;
            return TryClick(out outResult);
        }
        public bool TryClickByMouse<T>(out T outResult) where T : Component
        {
            m_rtf_point.position = Input.mousePosition;
            return TryClick(out outResult);
        }
        public bool TryClickByMouse<T>(out T[] outResult) where T : Component
        {
            m_rtf_point.position = Input.mousePosition;
            return TryClick(out outResult);
        }

        /// <summary>
        /// ������ �̵�
        /// </summary>
        public void MovePoint(Vector2 rectPos)
        {
            m_rtf_point.position = rectPos;
        }

        private void ClickLogic(bool checkMultiple)
        {
            ray = Camera.main.ScreenPointToRay(m_rtf_point.position);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_targetLayer))
            {
                ClickReaction clickReaction;
                m_tf_currentTarget = hit.collider.transform;

                if (m_tf_currentTarget.TryGetComponent(out clickReaction))
                {
                    clickReaction.Event_OnClick.Invoke();
                }

                if (checkMultiple)
                {
                    hits = Physics.RaycastAll(ray);
                    m_tf_currentTargets = new Transform[hits.Length];
                    for (int i = 0; i < hits.Length; i++)
                    {
                        m_TF_CurrentTargets[i] = hits[i].collider.transform;
                        if (m_TF_CurrentTargets[i].TryGetComponent(out clickReaction))
                        {
                            clickReaction.Event_OnClick.Invoke();
                        }
                    }
                }
            }
        }
    }
}