using System;
using UnityEngine;

namespace GLG.Ads
{
    public class AdsWrapper : MonoBehaviour, IManagedService
    {
        public bool useBanner = true;
        public bool useInter = true;
        public bool useRewarded = true;

        private IBannerProvider _banner;
        private IInterProvider _inter;
        private IRewardedProvider _rewarded;

        public IBannerProvider Banner => _banner;
        public IInterProvider Inter => _inter;
        public IRewardedProvider Rewarded => _rewarded;
        public Type Type => typeof(AdsWrapper);

        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            // banners
            if (useBanner)
            {
                _banner = CreateBanner();
            }
            //inters
            if (useInter)
            {
                _inter = CreateInter();
            }
            //rewarded
            if (useRewarded)
            {
                _rewarded = CreateRewarded();
            }
        }

        private IBannerProvider CreateBanner()
        {

            return null;
        }
        private IInterProvider CreateInter()
        {

            return null;
        }
        private IRewardedProvider CreateRewarded()
        {

            return null;
        }
    }
}
