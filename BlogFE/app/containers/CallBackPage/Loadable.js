/**
 *
 * Asynchronously loads the component for CallBackPage
 *
 */

import loadable from 'utils/loadable';

export default loadable(() => import('./index'));
